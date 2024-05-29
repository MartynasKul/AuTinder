using System.Collections;

namespace AuTinder.Models
{
    public class Route
    {
        public enum RouteStatus
        {
            Started,
            Completed
        }
        public int UserId { get; set; }
        public List<Ad> Deliveries { get; set; }
        public RouteStatus Status { get; set; }

        #region Constructors
        public Route() { }
        public Route(List<Ad> deliveries, RouteStatus status)
        {
            Deliveries = deliveries.ToList();
            Status = status;
        }
        public Route(int userid, List<Ad> deliveries, RouteStatus status) 
        {
            UserId = userid;
            Deliveries = deliveries.ToList();
            Status = status;
        }
        #endregion
    }
}
