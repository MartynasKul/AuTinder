namespace AuTinder.Models
{
    public enum OrderStatus
    {
        Paid,
        PendingPayment,
        Cancelled
    }
    public enum OrderType
    {
        Simple,
        Premium
    }

    public class Order
    {
        public decimal Price { get; set; }
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public OrderType OrderType { get; set; }

        public Ad Ad { get; set; }
    }
}
