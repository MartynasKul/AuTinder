namespace AuTinder.Models
{

    public enum DeliveryStatus
    {
        Cancelled,
        WaitingForDriver,
        Accepted,
        InProgress,
        Delivered
       
    }
    public class Delivery
    {
        public int Id { get; set; }
        public int Duration { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public int Length { get; set; }
        public string Address_to { get; set; }
        public string Address_from { get; set; }
    }
}
