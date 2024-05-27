using AuTinder.Models;
using System.Data.SqlClient;

namespace AuTinder.Repositories
{
    public class OrderRepo
    {

        public static void CreateOrder(Order order)
        {
            string query = @"
                INSERT INTO orders (Date, fk_order_status, fk_order_type, fk_user, fk_ad, fk_payment, fk_delivery, Price)
                VALUES (?Date, ?fk_order_status, ?fk_order_type, ?fk_user, ?fk_ad, ?fk_payment, ?fk_delivery, ?Price);";

            Sql.Insert(query, args =>
            {
                args.Add("?Date", order.Date);
                args.Add("?fk_order_status", order.OrderStatus);
                args.Add("?fk_order_type", order.OrderType);
                args.Add("?fk_user", 1);
                args.Add("?fk_ad", order.Ad.ID);
                args.Add("?fk_payment", 1);
                args.Add("?fk_delivery", 1);
                args.Add("?Price", order.Price);// Assuming fk_user is a foreign key to the user table
            });
        }

        public static List<Order> GetOrders()
        {
            string query = @"
    SELECT o.ID, o.Date, o.fk_order_status, o.fk_order_type, o.fk_user, o.fk_ad, o.fk_payment, o.fk_delivery,
           a.Id AS AdID, a.Description, a.Price, a.Ordered, 
           c.id AS CarId, c.make, c.model, c.fk_vechicle_type, c.year, c.fk_fuel_type, c.milage, c.color, 
           c.technical_inspection, c.fk_drive_types, c.fk_gear_box, c.power, c.fk_wheel_position, 
           c.outside_condition, c.additional_functions, c.value
    FROM orders o
    JOIN ad a ON o.fk_ad = a.Id
    JOIN car c ON a.Fk_Car = c.id;";

            var rows = Sql.Query(query);

            var ordersWithAds = Sql.MapAll<Order>(rows, (extractor, item) =>
            {
                item.Id = extractor.From<int>("ID");
                item.Date = extractor.From<DateTime>("Date");
                item.OrderStatus = MapToOrderStatus(extractor.From<int>("fk_order_status"));
                item.OrderType = MapToOrderType(extractor.From<int>("fk_order_type"));
                item.Ad = new Ad
                {
                    ID = extractor.From<int>("AdID"),
                    Description = extractor.From<string>("Description"),
                    Price = extractor.From<decimal>("Price"),
                    IsOrdered = extractor.From<bool>("Ordered"),
                    Car = new Car
                    {
                        Id = extractor.From<int>("CarId"),
                        Make = extractor.From<string>("make"),
                        Model = extractor.From<string>("model"),
                        BodyType = extractor.From<BodyType>("fk_vechicle_type"),
                        Year = extractor.From<DateTime>("year"),
                        FuelType = extractor.From<FuelType>("fk_fuel_type"),
                        Mileage = extractor.From<int>("milage"),
                        Color = extractor.From<string>("color"),
                        Inspection = extractor.From<DateTime>("technical_inspection"),
                        DriveWheels = extractor.From<DriveWheels>("fk_drive_types"),
                        Gearbox = extractor.From<Gearbox>("fk_gear_box"),
                        Power = extractor.From<int>("power"),
                        SteeringWheelLocation = extractor.From<SteeringWheelLocation>("fk_wheel_position"),
                        OutsideState = extractor.From<string>("outside_condition"),
                        ExtraFunc = extractor.From<string>("additional_functions"),
                        Rating = extractor.From<int>("value")
                    }
                };
            });

            return ordersWithAds;
        }

        private static OrderStatus MapToOrderStatus(int statusId)
        {
            switch (statusId)
            {
                case 1:
                    return OrderStatus.Paid;
                case 2:
                    return OrderStatus.PendingPayment;
                case 3:
                    return OrderStatus.Cancelled;
                // Add more cases as needed
                default:
                    throw new ArgumentException($"Unknown order status ID: {statusId}");
            }
        }

        private static OrderType MapToOrderType(int typeId)
        {
            switch (typeId)
            {
                case 1:
                    return OrderType.Simple;
                case 2:
                    return OrderType.Premium;
                // Add more cases as needed
                default:
                    throw new ArgumentException($"Unknown order type ID: {typeId}");
            }
        }
        public static int GetOrderStatusIdFromEnum(OrderStatus orderStatus)
        {
            switch (orderStatus)
            {
                case OrderStatus.Paid:
                    return 1; // Assuming 1 is the ID for "Paid" in the database
                case OrderStatus.PendingPayment:
                    return 2; // Assuming 2 is the ID for "Pending Payment" in the database
                case OrderStatus.Cancelled:
                    return 3; // Assuming 3 is the ID for "Cancelled" in the database
                default:
                    throw new ArgumentException("Unknown order status.");
            }
        }

        private static int MapToOrderStatus(OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Paid:
                    return 1;
                case OrderStatus.PendingPayment:
                    return 2;
                case OrderStatus.Cancelled:
                    return 3;
                // Add more cases as needed
                default:
                    throw new ArgumentException($"Unknown order status: {status}");
            }
        }

        public static List<Order> SelectOrders(OrderStatus? orderStatus)
        {
            string query = @"
    SELECT o.ID, o.Date, o.fk_order_status, o.fk_order_type, o.fk_user, o.fk_ad, o.fk_payment, o.fk_delivery,
           a.Id AS AdID, a.Description, a.Price, a.Ordered, 
           c.id AS CarId, c.make, c.model, c.fk_vechicle_type, c.year, c.fk_fuel_type, c.milage, c.color, 
           c.technical_inspection, c.fk_drive_types, c.fk_gear_box, c.power, c.fk_wheel_position, 
           c.outside_condition, c.additional_functions, c.value
    FROM orders o
    JOIN ad a ON o.fk_ad = a.Id
    JOIN car c ON a.Fk_Car = c.id
    WHERE @OrderStatus IS NULL OR o.fk_order_status = @OrderStatus;";

            var rows = Sql.Query(query, cmd =>
            {
                // Check if orderStatus is provided and convert it to a supported type
                if (orderStatus.HasValue)
                {
                    var orderStatusValue = MapToOrderStatus(orderStatus.Value);
                    cmd.Add("@OrderStatus", (int)orderStatusValue);
                }
            });

            var ordersWithAds = Sql.MapAll<Order>(rows, (extractor, item) =>
            {
                item.Id = extractor.From<int>("ID");
                item.Date = extractor.From<DateTime>("Date");
                item.OrderStatus = MapToOrderStatus(extractor.From<int>("fk_order_status"));
                item.OrderType = MapToOrderType(extractor.From<int>("fk_order_type"));
                item.Ad = new Ad
                {
                    ID = extractor.From<int>("AdID"),
                    Description = extractor.From<string>("Description"),
                    Price = extractor.From<decimal>("Price"),
                    IsOrdered = extractor.From<bool>("Ordered"),
                    Car = new Car
                    {
                        Id = extractor.From<int>("CarId"),
                        Make = extractor.From<string>("make"),
                        Model = extractor.From<string>("model"),
                        BodyType = extractor.From<BodyType>("fk_vechicle_type"),
                        Year = extractor.From<DateTime>("year"),
                        FuelType = extractor.From<FuelType>("fk_fuel_type"),
                        Mileage = extractor.From<int>("milage"),
                        Color = extractor.From<string>("color"),
                        Inspection = extractor.From<DateTime>("technical_inspection"),
                        DriveWheels = extractor.From<DriveWheels>("fk_drive_types"),
                        Gearbox = extractor.From<Gearbox>("fk_gear_box"),
                        Power = extractor.From<int>("power"),
                        SteeringWheelLocation = extractor.From<SteeringWheelLocation>("fk_wheel_position"),
                        OutsideState = extractor.From<string>("outside_condition"),
                        ExtraFunc = extractor.From<string>("additional_functions"),
                        Rating = extractor.From<int>("value")
                    }
                };
            });

            return ordersWithAds;
        }

        public static Order GetOrder(int id)
        {
            string query = @"
SELECT o.ID, o.Date, o.fk_order_status, o.fk_order_type, o.fk_user, o.fk_ad, o.fk_payment, o.fk_delivery,
       a.Id AS AdID, a.Description, a.Price, a.Ordered, 
       c.id AS CarId, c.make, c.model, c.fk_vechicle_type, c.year, c.fk_fuel_type, c.milage, c.color, 
       c.technical_inspection, c.fk_drive_types, c.fk_gear_box, c.power, c.fk_wheel_position, 
       c.outside_condition, c.additional_functions, c.value
FROM orders o
JOIN ad a ON o.fk_ad = a.Id
JOIN car c ON a.Fk_Car = c.id
WHERE o.ID = ?id;";

            var rows = Sql.Query(query, args =>
            {
                args.Add("?id", id);
            });

            var order = Sql.MapOne<Order>(rows, (extractor, item) =>
            {
                item.Id = extractor.From<int>("ID");
                item.Date = extractor.From<DateTime>("Date");
                item.OrderStatus = MapToOrderStatus(extractor.From<int>("fk_order_status"));
                item.OrderType = MapToOrderType(extractor.From<int>("fk_order_type"));
                item.Ad = new Ad
                {
                    ID = extractor.From<int>("AdID"),
                    Description = extractor.From<string>("Description"),
                    Price = extractor.From<decimal>("Price"),
                    IsOrdered = extractor.From<bool>("Ordered"),
                    Car = new Car
                    {
                        Id = extractor.From<int>("CarId"),
                        Make = extractor.From<string>("make"),
                        Model = extractor.From<string>("model"),
                        BodyType = extractor.From<BodyType>("fk_vechicle_type"),
                        Year = extractor.From<DateTime>("year"),
                        FuelType = extractor.From<FuelType>("fk_fuel_type"),
                        Mileage = extractor.From<int>("milage"),
                        Color = extractor.From<string>("color"),
                        Inspection = extractor.From<DateTime>("technical_inspection"),
                        DriveWheels = extractor.From<DriveWheels>("fk_drive_types"),
                        Gearbox = extractor.From<Gearbox>("fk_gear_box"),
                        Power = extractor.From<int>("power"),
                        SteeringWheelLocation = extractor.From<SteeringWheelLocation>("fk_wheel_position"),
                        OutsideState = extractor.From<string>("outside_condition"),
                        ExtraFunc = extractor.From<string>("additional_functions"),
                        Rating = extractor.From<int>("value")
                    }
                };
            });

            return order;
        }



    }
}
