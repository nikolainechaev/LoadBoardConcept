using FirebaseAdmin.Messaging;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/loads/")]
public class LoadController : ControllerBase
{
	private readonly FirestoreDb _firestore;
	public LoadController(FirestoreDb firestore)
	{
		_firestore = firestore;
	}
	[HttpPost("post")]
	public async Task<IActionResult> PostLoad([FromBody] Load load)
	{
		var collection = _firestore.Collection("loads");
		var docRef = await collection.AddAsync(load);

		await docRef.UpdateAsync("Id", docRef.Id);
		return Ok(new { Message = "Load posted successfully", Id = docRef.Id });

	}
	[HttpGet]
	public async Task<IActionResult> GetAllLoads()
	{
		var collection = _firestore.Collection("loads");
		var snapshot = await collection.GetSnapshotAsync();
		var loads = new List<Load>();

		foreach (var doc in snapshot.Documents)
		{
			var load = doc.ConvertTo<Load>();
			load.Id = doc.Id;
			loads.Add(load);
		}

		return Ok(loads);
	}
}