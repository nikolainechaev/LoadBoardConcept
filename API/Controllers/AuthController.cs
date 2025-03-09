using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
	private readonly FirestoreDb _firestore;

	public AuthController(FirestoreDb firestore)
	{
		_firestore = firestore;
	}



	[HttpPost("request-token")]
	public async Task<IActionResult> RequestToken([FromBody] TokenRequest request)
	{
		try
		{
			Console.WriteLine($"📌 Requesting Token for: {request.Email}");

			// ✅ Check if user exists in Firebase Authentication
			UserRecord user;
			try
			{
				user = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(request.Email);
			}
			catch (FirebaseAuthException)
			{
				Console.WriteLine($"⚠️ User not found in Firebase Auth: {request.Email}");
				return BadRequest(new { Message = "User does not exist. Register first." });
			}

			// ✅ Generate a Firebase Custom Token
			var customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(user.Uid);

			Console.WriteLine($"✅ Token generated for: {request.Email}");
			return Ok(new { Token = customToken });
		}
		catch (Exception ex)
		{
			Console.WriteLine($"❌ Error Generating Token: {ex.Message}");
			return BadRequest(new { Message = "Failed to generate token", Error = ex.Message });
		}
	}



	[HttpPost("store-user")]
	public async Task<IActionResult> StoreUser([FromBody] RegisterRequest request)
	{
		try
		{
			Console.WriteLine($"📌 Storing User in Firestore: {request.Email}");

			var userCollection = request.Role == "broker" ? "brokers" : "carriers";
			var userRef = _firestore.Collection(userCollection).Document(request.Email);

			var userData = new
			{
				Name = request.Name,
				Email = request.Email,
				Role = request.Role,
				Password = request.Password
			};

			await userRef.SetAsync(userData);
			Console.WriteLine($"✅ User stored in Firestore: {userCollection}, Email: {request.Email}");

			return Ok(new { Message = "User stored successfully" });
		}
		catch (Exception ex)
		{
			Console.WriteLine($"❌ Error Storing User: {ex.Message}");
			return BadRequest(new { Message = "Failed to store user", Error = ex.Message });
		}
	}

	// 📝 Models for requests
	public class RegisterRequest
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Role { get; set; } // "broker" or "carrier"
	}

	public class TokenRequest
	{
		public string Email { get; set; } // ✅ Fix: Add missing Email property
	}
}
