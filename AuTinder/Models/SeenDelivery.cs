using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace AuTinder.Models
{
    public class SeenDelivery
    {
        public int DeliveryId { get; set; }
        public int UserId { get; set; }
        public bool liked { get; set; }
        public Delivery Delivery { get; set; }

        #region Constructors
        public SeenDelivery() { }
        public SeenDelivery(int deliveryid, int userid, bool like)
        {
            DeliveryId = deliveryid;
            UserId = userid;
            liked = like;
        }

        public SeenDelivery(int deliveryid, int userid, bool like, Delivery delivery)
        {
            DeliveryId = deliveryid;
            UserId = userid;
            liked = like;
            this.Delivery = delivery;
        }
        #endregion
    }
}
