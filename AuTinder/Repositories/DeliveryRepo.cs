using AuTinder.Models;
using static AuTinder.Models.Delivery;
using static AuTinder.Models.Order;
using static AuTinder.Models.Ad;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
namespace AuTinder.Repositories
{
    public class DeliveryRepo
    {
        public static void InsertOrderAndDelivery(DateTime date, OrderStatus oStatus, OrderType oType, int userId, Ad Ad, int paymentId, Delivery delivery, decimal price,
            int deliveryId, int duration, DeliveryStatus deliveryStatus, int length, string addressTo, string addressFrom)
        {
            long orderId = InsertOrder(date, oStatus, oType, userId, Ad, paymentId, delivery, price);

            InsertDelivery(deliveryId, duration, deliveryStatus, length, addressTo, addressFrom);
        }

        private static long InsertOrder(DateTime date, OrderStatus oStatus, OrderType oType, int userId, Ad Ad, int paymentId, Delivery delivery, decimal price)
        {
            int orderType = GetOrderTypeIdFromEnum(oType);
            int orderStatus = GetOrderStatusIdFromEnum(oStatus);
            int deliveryStatus = GetDeliveryStatusIdFromEnum(delivery.Status);

            long orderId = 0; // Initialize orderId variable
            // Construct the SQL query for inserting into the orders table
            string orderQuery = $@"INSERT INTO orders (date, fk_order_status, fk_order_type, fk_user, fk_ad, fk_payment, fk_delivery, price)                  
                            VALUES (?date, ?fk_order_status, ?fk_order_type, ?fk_user, ?fk_ad, ?fk_payment, ?fk_delivery, ?price);
                            SELECT LAST_INSERT_ID()";


            // Add the parameters to the SQL query
            orderId = Sql.Insert(orderQuery, args =>
                {
                    args.Add("?date", date);
                    args.Add("?fk_order_status", orderStatus);
                    args.Add("?fk_order_type", orderType);
                    args.Add("?fk_user", userId);
                    args.Add("?fk_ad", Ad.ID);
                    args.Add("?fk_payment", paymentId);
                    args.Add("?fk_delivery", delivery.Id);
                    args.Add("?price", price);
                });
            return orderId; // Return the order ID
        }

        public static void CreateDelivery(Delivery delivery)
        {
            string query = @"
                INSERT INTO delivery (Duration, fk_delivery_status, Length, Address_to, Address_from)
                VALUES (?Duration, ?fk_delivery_status, ?Length, ?Address_to, ?Address_from);";

            Sql.Insert(query, args =>
            {
                args.Add("?Duration", delivery.Duration);
                args.Add("?fk_delivery_status", delivery.Status);
                args.Add("?Length", delivery.Length);
                args.Add("?Address_to", delivery.AddressTo);
                args.Add("?Address_from", delivery.AddressFrom);
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

        private static void InsertDelivery(int deliveryId, int duration, DeliveryStatus deliveryStatus, int length, string addressTo, string addressFrom)
        {
            string deliveryQuery = $@"INSERT INTO delivery (Duration, fk_delivery_status, Lenght, Address_to, Address_from)
                               VALUES (?Duration, ?fk_delivery_status, ?Lenght, ?Address_to, ?Address_from);";

            // Execute the query to insert into Ad table
            Sql.Insert(deliveryQuery, args =>
                {
                    args.Add("?Duration", duration);
                    args.Add("?fk_delivery_status", GetDeliveryStatusIdFromEnum(deliveryStatus));
                    args.Add("?Lenght", length);
                    args.Add("?Address_to", addressTo);
                    args.Add("?Address_from", addressFrom);
                });
        }


        public static void UpdateOrderAndDelivery(int deliveryId, int duration, DeliveryStatus deliveryStatus, int length, string addressTo, string addressFrom,
        int orderId, DateTime date, OrderStatus oStatus, OrderType oType, int userId, Ad Ad, int paymentId, Delivery delivery, decimal price)
        {
            long carId = UpdateOrder(orderId, date, oStatus, oType, userId, Ad, paymentId, delivery, price);

            UpdateDelivery(deliveryId, duration, deliveryStatus, length, addressTo, addressFrom);
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
                item.Status = MapToDeliveryStatus(extractor.From<int>("fk_delivery_status"));
                item.Length = extractor.From<int>("Length");
                item.AddressTo = extractor.From<string>("Address_to");
                item.AddressFrom = extractor.From<string>("Address_from");

            });

            return deliveries;
        }

        private static long UpdateOrder(int id, DateTime date, OrderStatus oStatus, OrderType oType, int userId, Ad Ad, int paymentId, Delivery delivery, decimal price)
        {
            int orderType = GetOrderTypeIdFromEnum(oType);
            int orderStatus = GetOrderStatusIdFromEnum(oStatus);
            int deliveryStatus = GetDeliveryStatusIdFromEnum(delivery.Status);

            long orderId = 0; // Initialize carId variable
                              // Construct the SQL query for inserting into the Car table
            string orderQuery = $@"UPDATE orders 
                    SET date = ?date,
                        fk_order_status = ?fk_order_status,
                        fk_order_type = ?fk_order_type,
                        fk_user = ?fk_user,
                        fk_ad = ?fk_ad,
                        fk_payment = ?fk_payment,
                        fk_delivery = ?fk_delivery,
                        price = ?price
                    WHERE id = ?id;";


            // Add the parameters to the SQL query
            orderId = Sql.Update(orderQuery, args =>
                {
                    args.Add("?id", id);
                    args.Add("?date", date);
                    args.Add("?fk_order_status", orderStatus);
                    args.Add("?fk_order_type", orderType);
                    args.Add("?fk_user", userId);
                    args.Add("?fk_ad", Ad.ID);
                    args.Add("?fk_payment", paymentId);
                    args.Add("?fk_delivery", delivery.Id);
                    args.Add("?price", price);
                });
            return orderId; // Return the order ID
        }

        private static void UpdateDelivery(int id, int duration, DeliveryStatus deliveryStatus, int length, string addressTo, string addressFrom)
        {
            string deliveryQuery = $@"UPDATE delivery 
                    SET duration = ?Duration,
                        fk_delivery_status = ?fk_delivery_status,
                        length = ?length,
                        address_to = ?address_to,
                        address_from = ?address_from
                    WHERE Id = ?id;";


            // Execute the query to insert into Ad table
            Sql.Update(deliveryQuery, args =>
            {
                args.Add("?id", id);
                args.Add("?Duration", duration);
                args.Add("?fk_delivery_status", GetDeliveryStatusIdFromEnum(deliveryStatus));
                args.Add("?length", length);
                args.Add("?address_to", addressTo);
                args.Add("?address_from", addressFrom);
            });
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

            int delivery_Status = GetDeliveryStatusIdFromEnum(delivery.Status);

            Sql.Update(query, args =>
            {
                args.Add("?id", id);
                args.Add("?Duration", delivery.Duration);
                args.Add("?fk_delivery_status", delivery_Status); // Assuming DeliveryStatus has an Id property
                args.Add("?Length", delivery.Length);
                args.Add("?Address_to", delivery.AddressTo);
                args.Add("?Address_from", delivery.AddressFrom);
            });
        }

        public static void DeleteDelivery(int deliveryId, int orderId)
        {
            string orderQuery = $@"DELETE FROM orders 
                    WHERE id = ?orderId;";


            // Execute the query to delete from orders table
            Sql.Delete(orderQuery, args =>
            {
                args.Add("?orderId", orderId);

            });

            string deliveryQuery = $@"DELETE FROM delivery 
                    WHERE Id = ?id;";


            // Execute the query to delete from delivery table
            Sql.Delete(orderQuery, args =>
            {
                args.Add("?id", deliveryId);

            });
        }

        private static int GetOrderTypeIdFromEnum(OrderType orderType)
        {
            switch (orderType)
            {
                case OrderType.Simple:
                    return 1; // Assuming 1 is the ID for Simple in the ordertypes table
                case OrderType.Premium:
                    return 2; // Assuming 2 is the ID for Premium in the ordertypes table
                default:
                    throw new ArgumentException("Unknown order type.");
            }
        }

        private static int GetOrderStatusIdFromEnum(OrderStatus orderStatus)
        {
            switch (orderStatus)
            {
                case OrderStatus.Paid:
                    return 1; // Assuming 1 is the ID for Paid in the orderstatus table
                case OrderStatus.PendingPayment:
                    return 2; // Assuming 2 is the ID for PendingPayment in the orderstatus table   
                case OrderStatus.Cancelled:
                    return 3; // Assuming 3 is the ID for Cancelled in the orderstatus table
                default:
                    throw new ArgumentException("Unknown order status.");
            }
        }

        private static int GetDeliveryStatusIdFromEnum(DeliveryStatus deliveryStatus)
        {
            switch (deliveryStatus)
            {
                case DeliveryStatus.Delivered:
                    return 1; // Assuming 1 is the ID for Delivered in the deliverystatus table
                case DeliveryStatus.InProgress:
                    return 2; // Assuming 2 is the ID for InProgress in the deliverystatus table
                case DeliveryStatus.Accepted:
                    return 3; // Assuming 3 is the ID for Accepted in the deliverystatus table
                case DeliveryStatus.WaitingForDriver:
                    return 4; // Assuming 4 is the ID for WaitingForDriver in the deliverystatus table
                case DeliveryStatus.Cancelled:
                    return 5; // Assuming 5 is the ID for Cancelled in the deliverystatus table
                default:
                    throw new ArgumentException("Unknown delivery status.");
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

        public static List<Delivery> GetAllDeliveries()
        {
            string query = @"
        SELECT d.id as delivery_id, d.duration, d.fk_delivery_status, d.length, d.address_to, d.address_from,
                o.id as order_id, o.date, o.fk_order_status, o.fk_order_type, o.fk_user, o.fk_ad, o.fk_payment, o.fk_delivery, o.price
        FROM delivery d
        JOIN orders o ON o.fk_delivery = d.ID";

            var rows = Sql.Query(query);
            var deliveriesWithOrders = Sql.MapAll<Delivery>(rows, (extractor, item) =>
            {
                item.Id = extractor.From<int>("delivery_id");
                item.Duration = extractor.From<int>("duration");
                item.Status = extractor.From<DeliveryStatus>("fk_delivery_status");
                item.Length = extractor.From<int>("length");
                item.AddressTo = extractor.From<string>("address_to");
                item.AddressFrom = extractor.From<string>("address_from");
                item.Order = new Order
                {
                    Id = extractor.From<int>("order_id"),
                    Date = extractor.From<DateTime>("date"),
                    OrderStatus = extractor.From<OrderStatus>("fk_order_status"),
                    OrderType = extractor.From<OrderType>("fk_order_type"),
                    Price = extractor.From<decimal>("price"),
                    /*
                                    AdId = extractor.From<int>("fk_ad"),
                                    PaymentId = extractor.From<int>("fk_payment")
                    */
                };
            });

            return deliveriesWithOrders;
        }

        public static Delivery GetDeliveryAndOrderById(int deliveryId)
        {
            string query = @"
        SELECT d.id as delivery_id, d.duration, d.fk_delivery_status, d.length, d.address_to, d.address_from,
                o.id as order_id, o.date, o.fk_order_status, o.fk_order_type, o.fk_user, o.fk_ad, o.fk_payment, o.fk_delivery, o.price
        FROM delivery d
        JOIN orders o ON o.fk_delivery = d.ID WHERE d.ID = ?id";

            var rows = Sql.Query(query, args =>
            {
                args.Add("?id", deliveryId);

            });
            var deliveriesWithOrders = Sql.MapOne<Delivery>(rows, (extractor, item) =>
            {
                item.Id = extractor.From<int>("delivery_id");
                item.Duration = extractor.From<int>("duration");
                item.Status = extractor.From<DeliveryStatus>("fk_delivery_status");
                item.Length = extractor.From<int>("length");
                item.AddressTo = extractor.From<string>("address_to");
                item.AddressFrom = extractor.From<string>("address_from");
                item.Order = new Order
                {
                    Id = extractor.From<int>("order_id"),
                    Date = extractor.From<DateTime>("date"),
                    OrderStatus = extractor.From<OrderStatus>("fk_order_status"),
                    OrderType = extractor.From<OrderType>("fk_order_type"),
                    Price = extractor.From<decimal>("price"),
                    /*
                                AdId = extractor.From<int>("fk_ad"),
                                PaymentId = extractor.From<int>("fk_payment")
                    */
                };
            });

            return deliveriesWithOrders;
        }

        public static List<SeenDelivery> GetSeenDeliveries(int userId)
        {
            string query = @"
            SELECT sd.fk_delivery, sd.fk_user, sd.liked
            FROM seendelivery sd
            WHERE sd.fk_user = ?userId;";

            var rows = Sql.Query(query, args =>
            {
                args.Add("?userId", userId);

            });
            var seenDeliveries = Sql.MapAll<SeenDelivery>(rows, (extractor, item) =>
            {
                item.DeliveryId = extractor.From<int>("fk_delivery");
                item.UserId = extractor.From<int>("fk_user");
                item.liked = extractor.From<bool>("liked");
            });

            foreach (var delivery in seenDeliveries)
            {
                delivery.Delivery = GetDeliveryAndOrderById(delivery.DeliveryId);
            }

            return seenDeliveries;
        }


        public static List<Delivery> GetUserPreferences(int userId)
        {
            string query = @"
        SELECT d.id as delivery_id, d.duration, d.fk_delivery_status, d.length, d.address_to, d.address_from,
                o.id as order_id, o.date, o.fk_order_status, o.fk_order_type, o.fk_user, o.fk_ad, o.fk_payment, o.fk_delivery, o.price
        FROM delivery d
        JOIN orders o ON o.fk_delivery = d.ID WHERE d.ID = ?userId";

            var rows = Sql.Query(query, args =>
            {
                args.Add("?userId", userId);

            });
            var deliveries = Sql.MapAll<Delivery>(rows, (extractor, item) =>
            {
                item.Id = extractor.From<int>("delivery_id");
                item.Duration = extractor.From<int>("duration");
                item.Status = extractor.From<DeliveryStatus>("fk_delivery_status");
                item.Length = extractor.From<int>("length");
                item.AddressTo = extractor.From<string>("address_to");
                item.AddressFrom = extractor.From<string>("address_from");
                item.Order = new Order
                {
                    Id = extractor.From<int>("order_id"),
                    Date = extractor.From<DateTime>("date"),
                    OrderStatus = extractor.From<OrderStatus>("fk_order_status"),
                    OrderType = extractor.From<OrderType>("fk_order_type"),
                    Price = extractor.From<decimal>("price"),
                    /*
                                AdId = extractor.From<int>("fk_ad"),
                                PaymentId = extractor.From<int>("fk_payment")
                    */
                };
            });

            return deliveries;
        }


        public static void SaveSeenDeliveries(List<SeenDelivery> seenDeliveries)
        {
            foreach (var seenDelivery in seenDeliveries)
            {
                string query = @"
                    INSERT INTO seendelivery (fk_user, fk_ad, liked)
                    VALUES (?UserId, ?DeliveryId, ?Liked);";

                Sql.Insert(query, args =>
                {
                    args.Add("?UserId", seenDelivery.UserId);
                    args.Add("?DeliveryId", seenDelivery.DeliveryId);
                    args.Add("?Liked", seenDelivery.liked);
                });
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
                item.DeliveryId = extractor.From<int>("fk_delivery");
                item.UserId = extractor.From<int>("fk_user");
                item.liked = extractor.From<bool>("liked");
            });

            foreach (var seenDelivery in seenDeliveriesRows)
            {
                if (seenDelivery.liked)
                {
                    seenDelivery.Delivery = DeliveryRepo.GetDelivery(seenDelivery.DeliveryId);
                    seenDeliveries.Add(seenDelivery);
                }
            }

            return seenDeliveries;
        }

        public static void AddDeliveyToRoute(int deliveyId, int userID, int routeStatus)
        {
            string query = @"
                    INSERT INTO Route (fk_routestatus, fk_delivery, fk_user)
                    VALUES (?RouteStatus, ?DeliveryId, ?userId);";

            Sql.Insert(query, args =>
            {
                args.Add("?RouteStatus", routeStatus);
                args.Add("?DeliveryId", deliveyId);
                args.Add("?userId", userID);
            });
        }

        public static DeliveryRoute GetRouteForFinalDelivery(int userId)
        {

            string query = @"
                SELECT fk_delivery
                FROM route
                WHERE fk_user = ?userId AND fk_routestatus = ?status;";

            var rows = Sql.Query(query, args =>
            {
                args.Add("?userId", userId);
                args.Add("?status", 2);
            });
            List<DeliveryRoute> routes = Sql.MapAll<DeliveryRoute>(rows, (extractor, item) =>
            {
                item.DeliveryId = extractor.From<int>("fk_delivery");
            });

            List<Delivery> deliveries = new List<Delivery>();

            foreach (DeliveryRoute rout in routes)
            {
                deliveries.Add(GetDelivery(rout.DeliveryId));
            }
            DeliveryRoute route = new DeliveryRoute(deliveries);
            return route;
        }

        public static void updateDeliveryRoute(int userId)
        {
            string query = @"
            UPDATE route
            SET fk_routestatus = ?status
            WHERE fk_user = ?userId";

            Sql.Update(query, args =>
            {
                args.Add("?userId", userId);
                args.Add("?status", 1);
            });
        }

        public static void RemoveDeliveryFromRoute(int deliveryID) 
        {
            string query = @"
                DELETE FROM route
                WHERE fk_delivery = ?deliveryId";

            Sql.Delete(query, args =>
            {
                args.Add("?deliveryId", deliveryID);
            });
        }
    }
}
