namespace AuTinder.Models
{
    public class SeenDelivery
    {
        public int DeliveryID { get; set; }
        public int UserID { get; set; }
        public bool Liked {  get; set; }
        public Delivery delivery { get; set; }

        public SeenDelivery() { }

        public SeenDelivery(int deliveryID, int userID, bool liked, Delivery delivery)
        {
            DeliveryID = deliveryID;
            UserID = userID;
            Liked = liked;
            this.delivery = delivery;
        }

    }
}
