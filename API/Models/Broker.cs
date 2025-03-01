using Google.Cloud.Firestore;

[FirestoreData]
public class Broker
{
	[FirestoreProperty]
	public string Id { get; set; }
	[FirestoreProperty]
	public string Name { get; set; }
	[FirestoreProperty]
	public string Company { get; set; }
}