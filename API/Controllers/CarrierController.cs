using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/carriers")]
public class CarrierController : ControllerBase
{
	private readonly FirestoreDb _firestore;

	public CarrierController(FirestoreDb firestore)
	{
		_firestore = firestore;
	}

	// Create a new carrier
	[HttpPost]
	public async Task<IActionResult> CreateCarrier([FromBody] Carrier carrier)
	{
		var collection = _firestore.Collection("carriers");
		var docRef = await collection.AddAsync(carrier);
		return Ok(new { Id = docRef.Id });
	}

	// Get all carriers
	[HttpGet]
	public async Task<IActionResult> GetCarriers()
	{
		var collection = _firestore.Collection("carriers");
		var snapshot = await collection.GetSnapshotAsync();
		var carriers = new List<Carrier>();

		foreach (var doc in snapshot.Documents)
		{
			var carrier = doc.ConvertTo<Carrier>();
			carrier.Id = doc.Id;
			carriers.Add(carrier);
		}

		return Ok(carriers);
	}

	// Get a carrier by ID
	[HttpGet("{id}")]
	public async Task<IActionResult> GetCarrierById(string id)
	{
		var doc = await _firestore.Collection("carriers").Document(id).GetSnapshotAsync();
		if (!doc.Exists) return NotFound("Carrier not found.");

		var carrier = doc.ConvertTo<Carrier>();
		carrier.Id = doc.Id;
		return Ok(carrier);
	}

	// Update a carrier
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateCarrier(string id, [FromBody] Carrier carrier)
	{
		var docRef = _firestore.Collection("carriers").Document(id);
		await docRef.SetAsync(carrier, SetOptions.Overwrite);
		return Ok(new { Message = "Carrier updated successfully." });
	}

	// Delete a carrier
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteCarrier(string id)
	{
		var docRef = _firestore.Collection("carriers").Document(id);
		await docRef.DeleteAsync();
		return Ok(new { Message = "Carrier deleted successfully." });
	}
}
