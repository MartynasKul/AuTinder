namespace AuTinder.Models
{
    public class Delivery
    {
        public enum DeliveryStatus
        {
            Delivered,
            InProgress,
            Accepted,
            WaitingForDriver,
            Cancelled
        }

        public int Id { get; set; }
        public int Duration { get; set; }
        public DeliveryStatus Status { get; set; }
        public int Length { get; set; }
        public string AddressFrom { get; set; }
        public string AddressTo { get; set; }
        public User User { get; set; }
        public Order Order { get; set; }

        #region Constructors
        public Delivery() { }
        public Delivery(int id, int duration, DeliveryStatus status, int length, string addressfrom, string addressto, Order order) 
        {
            Id = id;
            Duration = duration;
            Status = status;
            Length = length;
            AddressFrom = addressfrom;
            AddressTo = addressto;
            Order = order;
        }
        public Delivery(int id, int duration, DeliveryStatus status, int length, string addressfrom, string addressto, User user, Order order)
        {
            Id = id;
            Duration = duration;
            Status = status;
            Length = length;
            AddressFrom = addressfrom;
            AddressTo = addressto;
            User = user;
            Order = order;
        }
        #endregion

        public int CompareDeliveries(Delivery other)
    {
            if (other == null)
                return 0;
       
            if (this.Order == null || other.Order == null)
                return 0;


            int matchCount = 0;

            if ((this.Duration - other.Duration) <= 60) matchCount++;
            if ((this.Length - other.Length) <= 30) matchCount++;
            if ((this.Order.Price - other.Order.Price) <= 200) matchCount++;

            return matchCount;
    }

        public List<int> CompareRpeatsInDeliveries(Delivery other, List<int> repeats)
    {
            if (other == null)
                return repeats;

            if (this.Order == null || other.Order == null)
                return repeats;


            if ((this.Duration - other.Duration) <= 60) repeats[0]++;
            if ((this.Length - other.Length) <= 30) repeats[1]++;
            if (Math.Abs(this.Order.Price - other.Order.Price) <= 200) repeats[2]++;

            return repeats;
        }
    }
}
