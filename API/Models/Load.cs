using Google.Cloud.Firestore;

[FirestoreData]
public class Load
{
	[FirestoreProperty]
	public string Id { get; set; }
	[FirestoreProperty]
	public string BrokerId { get; set; }
	[FirestoreProperty]
	public string PickupLocation { get; set; }
	[FirestoreProperty]
	public string DropoffLocation { get; set; }
	[FirestoreProperty]
	public string Price { get; set; }
	[FirestoreProperty]
	public string Weight { get; set; }
	[FirestoreProperty]
	public bool IsBooked { get; set; } = false;
	[FirestoreProperty]
	public string? CarrierId { get; set; } // null if unbooked
}