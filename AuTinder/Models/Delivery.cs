namespace AuTinder.Models
{

    public enum DeliveryStatus
    {
        Delivered,
        InProgress,
        Accepted,
        WaitingForDriver,
        Cancelled
    }
    public class Delivery
    {
        public int Durateion { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public int Length { get; set; }
        public string Address_to { get; set; }
        public string Address_from { get; set; }
    }
}
