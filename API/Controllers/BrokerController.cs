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
}