namespace AuTinder.Models
{
    public class Ad
    {
        public string Description {  get; set; }
        public decimal Price { get; set; }
        public bool IsOrdered { get; set; }
        public Car Car { get; set; }

        #region Constructors
        public Ad() { }
        public Ad(string description, decimal price, bool isOrdered, Car car) 
        {
            Description = description;
            Price = price;
            IsOrdered = isOrdered;
            Car = car;
        }
        public Ad(string description, decimal price, Car car) 
        {
            Description = description;
            Price = price;
            Car = car;
            IsOrdered=false;
        }

        #endregion
    }
}
