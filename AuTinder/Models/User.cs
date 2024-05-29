using Mysqlx.Expr;
using PayPalCheckoutSdk.Orders;

namespace AuTinder.Models
{
    public class User
    {
        public int Id { get; set; }
        public bool Driver { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int SearchDistance { get; set; }
        public DateTime Date { get; set; }
        public int AdCount { get; set; }
        public int OrderCount { get; set; }

        public User(int id, bool driver, string email, string name, string surname, string phone, string address, int searchDistance, int adCount, int orderCount, DateTime Date)
        {
            Id = id;
            Driver = driver;
            Email = email;
            Name = name;
            Surname = surname;
            Phone = phone;
            Address = address;
            SearchDistance = searchDistance;
            AdCount = adCount;
            OrderCount = orderCount;
            this.Date = Date;
        }
        public User() { }
    }
}
