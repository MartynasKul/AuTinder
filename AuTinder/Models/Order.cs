using System.Security.Policy;

namespace AuTinder.Models
{
    public enum OrderStatus
    {
        Paid = 1,
        PendingPayment = 2,
        Cancelled = 3
    }
    public enum OrderType
    {
        Simple = 1,
        Premium = 2
    }

    public class Order
    {
        public decimal Price { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public OrderType OrderType { get; set; }
        public Delivery Delivery { get; set; }
        public Payment Payment { get; set; }
        public User User { get; set; }
        public Ad Ad { get; set; }
        public int AverageTime { get; set; }
        public string PayPalOrderId { get; set; }

#region Constructors
        public Order() { }
        public Order(int id, DateTime date, OrderStatus orderstatus, OrderType ordertype, decimal price)
        {
            Id = id;
            Date = date;
            OrderStatus = orderstatus;
            OrderType = ordertype;
            Price = price;
        }
        public Order(int id, DateTime date, OrderStatus orderstatus, OrderType ordertype, decimal price, Ad ad)
        {
            Id = id;
            Date = date;
            OrderStatus = orderstatus;
            OrderType = ordertype;
            Price = price;
            Ad = ad;
        }
        #endregion

    }
}
