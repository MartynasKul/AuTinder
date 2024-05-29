namespace AuTinder.Models
{
    public class Ad
    {
        public int ID { get; set; }
        public string Description {  get; set; }
        public decimal Price { get; set; }
        public bool IsOrdered { get; set; }
        public string Address { get; set; }
        public Car Car { get; set; }

        #region Constructors
        public Ad() { }
        public Ad(int id, string description, decimal price, bool isOrdered, Car car) 
        {
            ID = id;
            Description = description;
            Price = price;
            IsOrdered = isOrdered;
            Car = car;
        }
        public Ad(int id, string description, decimal price, Car car) 
        {
            ID = id;
            Description = description;
            Price = price;
            Car = car;
            IsOrdered=false;
        }

        #endregion
    }
}
