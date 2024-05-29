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
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public OrderType OrderType { get; set; }
        public decimal Price { get; set; }

        public Ad Ad { get; set; }

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
