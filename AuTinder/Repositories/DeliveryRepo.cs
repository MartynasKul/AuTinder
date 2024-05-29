using AuTinder.Models;
using System.Data.SqlClient;
namespace AuTinder.Repositories
{
    public class DeliveryRepo
    {

        public static void CreateDelivery(Delivery delivery)
        {
            string query = @"
                INSERT INTO delivery (Duration, fk_delivery_status, Length, Address_to, Address_from)
                VALUES (?Duration, ?fk_delivery_status, ?Length, ?Address_to, ?Address_from);";

            Sql.Insert(query, args =>
            {
                args.Add("?Duration", delivery.Duration);
                args.Add("?fk_delivery_status", delivery.DeliveryStatus);
                args.Add("?Length", delivery.Length);
                args.Add("?Address_to", delivery.Address_to);
                args.Add("?Address_from", delivery.Address_from);
            });
        }

        public static int GetLastInsertedDeliveryId()
        {
            string query = @"
        SELECT ID
        FROM delivery
        ORDER BY ID DESC
        LIMIT 1;";

            // Execute the query to retrieve the last inserted ID
            var result = Sql.Query(query);

            // Check if any result is returned
            if (result.Count == 0)
            {
                throw new Exception("No delivery records found.");
            }

            // Extract the last inserted ID from the result
            int lastInsertedId = Convert.ToInt32(result[0]["ID"]);

            return lastInsertedId;
        }


        public static Delivery GetDelivery(int id)
        {
            string query = @"
                SELECT d.ID, d.Duration, d.fk_delivery_status, d.Length, d.Address_to, d.Address_from
                FROM delivery d
                WHERE d.ID = ?id";

            var rows = Sql.Query(query, args =>
            {
                args.Add("?id", id);

            });

            var deliveries = Sql.MapOne<Delivery>(rows, (extractor, item) =>
            {
                item.Id = extractor.From<int>("ID");
                item.Duration = extractor.From<int>("Duration");
                item.DeliveryStatus =MapToDeliveryStatus(extractor.From<int>("fk_delivery_status"));
                item.Length = extractor.From<int>("Length");
                item.Address_to = extractor.From<string>("Address_to");
                item.Address_from = extractor.From<string>("Address_from");

            });

            return deliveries;
        }

        public static void UpdateDelivery(Delivery delivery, int id)
        {
            string query = @"
            UPDATE delivery
            SET Duration = ?Duration,
                fk_delivery_status = ?fk_delivery_status,
                Length = ?Length,
                Address_to = ?Address_to,
                Address_from = ?Address_from
            WHERE ID = ?id";

            int delivery_Status = GetDeliveryStatusFromEnum(delivery.DeliveryStatus);

            Sql.Update(query, args =>
            {
                args.Add("?id", id);
                args.Add("?Duration", delivery.Duration);
                args.Add("?fk_delivery_status", delivery_Status); // Assuming DeliveryStatus has an Id property
                args.Add("?Length", delivery.Length);
                args.Add("?Address_to", delivery.Address_to);
                args.Add("?Address_from", delivery.Address_from);
            });
        }

        private static int GetDeliveryStatusFromEnum(DeliveryStatus delivery)
        {
            switch (delivery)
            {
                case DeliveryStatus.Delivered:
                    return 1; // Assuming 1 is the ID for Gasoline in the fueltypes table
                case DeliveryStatus.InProgress:
                    return 2; // Assuming 2 is the ID for Diesel in the fueltypes table
                case DeliveryStatus.Accepted:
                    return 3; // Assuming 2 is the ID for Diesel in the fueltypes table
                case DeliveryStatus.WaitingForDriver:
                    return 4; // Assuming 2 is the ID for Diesel in the fueltypes table
                case DeliveryStatus.Cancelled:
                    return 5;
                default:
                    throw new ArgumentException("Unknown fuel type.");
            }
        }

        private static DeliveryStatus MapToDeliveryStatus(int statusId)
        {
            switch (statusId)
            {
                case 1:
                    return DeliveryStatus.Delivered;
                case 2:
                    return DeliveryStatus.InProgress;
                case 3:
                    return DeliveryStatus.Accepted;
                case 4:
                    return DeliveryStatus.WaitingForDriver;
                case 5:
                    return DeliveryStatus.Cancelled;
                // Add more cases as needed
                default:
                    throw new ArgumentException($"Unknown delivery status ID: {statusId}");
            }
        }

        public static List<SeenDelivery> GetLikedDeliveries(int userId)
        {
            List<SeenDelivery> seenDeliveries = new List<SeenDelivery>();
            string query = @"
                SELECT sd.fk_delivery, sd.fk_user, sd.liked
                FROM seendelivery sd
                WHERE sd.fk_user = ?userId;";

            var rows = Sql.Query(query, args =>
            {
                args.Add("?userId", userId);
            });

            var seenDeliveriesRows = Sql.MapAll<SeenDelivery>(rows, (extractor, item) =>
            {
                item.DeliveryID = extractor.From<int>("fk_delivery");
                item.UserID = extractor.From<int>("fk_user");
                item.Liked = extractor.From<bool>("liked");
            });

            foreach (var seenDelivery in seenDeliveriesRows)
            {
                if (seenDelivery.Liked)
                {
                    seenDelivery.delivery = DeliveryRepo.GetDelivery(seenDelivery.DeliveryID);
                    seenDeliveries.Add(seenDelivery);
                }
            }

            return seenDeliveries;
        }
    }
}
