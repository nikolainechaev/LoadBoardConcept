using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/brokers")]

public class BrokerController : ControllerBase
{
	private readonly FirestoreDb _firestore;
	public BrokerController(FirestoreDb firestore)
	{
		_firestore = firestore;
	}

	[HttpPost]
	public async Task<IActionResult> CreateBroker([FromBody] Broker broker)
	{
		var collection = _firestore.Collection("brokers");
		var docRef = await collection.AddAsync(broker);
		return Ok(new { Id = docRef.Id });
	}

	[HttpGet]
	public async Task<IActionResult> GetBrokers()
	{
		var collection = _firestore.Collection("brokers");
		var snapshot = await collection.GetSnapshotAsync();
		var brokers = new List<Broker>();

		foreach (var doc in snapshot.Documents)
		{
			var broker = doc.ConvertTo<Broker>();
			broker.Id = doc.Id;
			brokers.Add(broker);
		}
		return Ok(brokers);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetBrokerById(string id)
	{
		var doc = await _firestore.Collection("brokers").Document(id).GetSnapshotAsync();
		if (!doc.Exists) return NotFound("Carrier not found.");

		var broker = doc.ConvertTo<Broker>();
		broker.Id = doc.Id;
		return Ok(broker);
	}
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateBroker(string id, [FromBody] Broker broker)
	{
		var docRef = _firestore.Collection("brokers").Document(id);
		await docRef.SetAsync(broker, SetOptions.Overwrite);
		return Ok(new { Message = "Broker updated successfully." });
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteBroker(string id)
	{
		var docRef = _firestore.Collection("brokers").Document(id);
		await docRef.DeleteAsync();
		return Ok(new { message = "Broker has been deleted" });
	}

}