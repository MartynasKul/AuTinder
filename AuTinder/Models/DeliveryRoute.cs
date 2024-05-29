using System.Collections;

namespace AuTinder.Models
{
    public class DeliveryRoute
    {
        public enum RouteStatus
        {
            Started,
            Completed
        }
        public int DeliveryId {  get; set; }
        public int UserId { get; set; }
        public List<Delivery> Deliveries { get; set; }
        public RouteStatus Status { get; set; }

        #region Constructors
        public DeliveryRoute() { }

        public DeliveryRoute(int deliveryId)
        {
            DeliveryId = deliveryId;
;
        }

        public DeliveryRoute(List<Delivery> deliveries)
        {
            Deliveries = deliveries;
        }

        public DeliveryRoute(int deliveryId, int userId, List<Delivery> deliveries, RouteStatus status)
        {
            DeliveryId = deliveryId;
            UserId = userId;
            Deliveries = deliveries;
            Status = status;
        }

        public DeliveryRoute(List<Delivery> deliveries, RouteStatus status)
        {
            Deliveries = deliveries.ToList();
            Status = status;
        }
        public DeliveryRoute(int userid, List<Delivery> deliveries, RouteStatus status) 
        {
            UserId = userid;
            Deliveries = deliveries.ToList();
            Status = status;
        }
        #endregion
    }
}
