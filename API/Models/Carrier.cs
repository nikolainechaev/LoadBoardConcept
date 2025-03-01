using Google.Cloud.Firestore;

[FirestoreData]
public class Carrier
{
	[FirestoreProperty]
	public string Id { get; set; }
	[FirestoreProperty]
	public string Name { get; set; }
	[FirestoreProperty]
	public string TruckType { get; set; }
}
