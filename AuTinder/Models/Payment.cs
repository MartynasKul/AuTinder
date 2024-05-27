namespace AuTinder.Models
{

    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public bool Paid { get; set; }
    }
}
