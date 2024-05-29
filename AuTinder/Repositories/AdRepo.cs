using AuTinder.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

public class AdRepo
{
    public static void InsertCarAndAd(string make, string model, BodyType bodyType, DateTime year, FuelType fuelType,
     int mileage, string color, DateTime inspection, DriveWheels driveWheels, Gearbox gearbox,
     int power, SteeringWheelLocation steeringWheelLocation, string outsideState, string extraFunc, float rating,
     string description, decimal price, bool isOrdered)
    {
        long carId = InsertCar(make, model, bodyType, year, fuelType, mileage, color, inspection, driveWheels, gearbox,
                              power, steeringWheelLocation, outsideState, extraFunc, rating);

        InsertAd(description, price, isOrdered, carId);
    }

    private static long InsertCar(string make, string model, BodyType bodyType, DateTime year, FuelType fuelType,
     int mileage, string color, DateTime inspection, DriveWheels driveWheels, Gearbox gearbox,
     int power, SteeringWheelLocation steeringWheelLocation, string outsideState, string extraFunc, float rating)
    {
        int fuelTypeId = GetFuelTypeIdFromEnum(fuelType); // Retrieve the ID from the enum value
        int bodyTypeId = GetBodyTypeIdFromEnum(bodyType);
        int driveWheelsId = GetDriveWheelsIdFromEnum(driveWheels);
        int gearBox = GetGearboxIdFromEnum(gearbox);
        int steeringwheellocation = GetSteeringIdFromEnum(steeringWheelLocation);

        long carId = 0; // Initialize carId variable
        // Construct the SQL query for inserting into the Car table
        string carQuery = $@"INSERT INTO car (make, model, fk_vechicle_type, year, fk_fuel_type, milage, color, technical_inspection, fk_drive_types, fk_gear_box, power, fk_wheel_position, outside_condition, additional_functions, value)                  
                        VALUES (?make, ?model, ?fk_vechicle_type, ?year, ?fk_fuel_type, ?milage, ?color, ?technical_inspection, ?fk_drive_types, ?fk_gear_box, ?power, ?fk_wheel_position, ?outside_condition, ?additional_functions, ?value);
                        SELECT LAST_INSERT_ID()";


        // Add the parameters to the SQL query
        carId = Sql.Insert(carQuery, args =>
        {
            args.Add("?make", make);
            args.Add("?model", model);
            args.Add("?fk_vechicle_type", bodyTypeId); // Assuming you have a foreign key for body type
            args.Add("?year", year);
            args.Add("?fk_fuel_type", fuelTypeId); // Assuming you have a foreign key for fuel type
            args.Add("?milage", mileage);
            args.Add("?color", color);
            args.Add("?technical_inspection", inspection);
            args.Add("?fk_drive_types", driveWheelsId); // Assuming you have a foreign key for drive wheels
            args.Add("?fk_gear_box", gearBox); // Assuming you have a foreign key for gearbox
            args.Add("?power", power);
            args.Add("?fk_wheel_position", steeringwheellocation); // Assuming you have a foreign key for wheel position
            args.Add("?outside_condition", outsideState);
            args.Add("?additional_functions", extraFunc);
            args.Add("?value", rating);
        });
        return carId; // Return the car ID
    }

    private static void InsertAd(string description, decimal price, bool isOrdered, long carId)
    {
        
        string adQuery = $@"INSERT INTO ad (Description, Price, Ordered, Fk_Car, fk_user, Address)
                           VALUES (?Description, ?Price, ?Ordered, ?Fk_Car, ?fk_user, ?Address);";

        // Execute the query to insert into Ad table
        Sql.Insert(adQuery, args =>
        {
            args.Add("?Description", description);
            args.Add("?Price", price);
            args.Add("?Ordered", isOrdered);
            args.Add("?Fk_Car", carId);
            args.Add("?fk_user", 1); // Assuming fk_user is a foreign key to the user table
            args.Add("?Address", "Respublikos g. 49, Telšiai, 87130 Telšių r. sav.");
        });
    }


    public static void UpdateCarAndAd(int adid, int carid, string make, string model, BodyType bodyType, DateTime year, FuelType fuelType,
     int mileage, string color, DateTime inspection, DriveWheels driveWheels, Gearbox gearbox,
     int power, SteeringWheelLocation steeringWheelLocation, string outsideState, string extraFunc, float rating,
     string description, decimal price, bool isOrdered)
    {
        long carId = UpdateCar(carid, make, model, bodyType, year, fuelType, mileage, color, inspection, driveWheels, gearbox,
                              power, steeringWheelLocation, outsideState, extraFunc, rating);

        UpdateAd(adid, description, price, isOrdered);
    }

    public static long UpdateCar(int id, string make, string model, BodyType bodyType, DateTime year, FuelType fuelType,
     int mileage, string color, DateTime inspection, DriveWheels driveWheels, Gearbox gearbox,
     int power, SteeringWheelLocation steeringWheelLocation, string outsideState, string extraFunc, float rating)
    {
        int fuelTypeId = GetFuelTypeIdFromEnum(fuelType); // Retrieve the ID from the enum value
        int bodyTypeId = GetBodyTypeIdFromEnum(bodyType);
        int driveWheelsId = GetDriveWheelsIdFromEnum(driveWheels);
        int gearBox = GetGearboxIdFromEnum(gearbox);
        int steeringwheellocation = GetSteeringIdFromEnum(steeringWheelLocation);

        long carId = 0; // Initialize carId variable
        // Construct the SQL query for inserting into the Car table
        string carQuery = $@"UPDATE car 
                    SET make = ?make, 
                        model = ?model, 
                        fk_vechicle_type = ?fk_vechicle_type, 
                        year = ?year, 
                        fk_fuel_type = ?fk_fuel_type, 
                        milage = ?milage, 
                        color = ?color, 
                        technical_inspection = ?technical_inspection, 
                        fk_drive_types = ?fk_drive_types, 
                        fk_gear_box = ?fk_gear_box, 
                        power = ?power, 
                        fk_wheel_position = ?fk_wheel_position, 
                        outside_condition = ?outside_condition, 
                        additional_functions = ?additional_functions, 
                        value = ?value
                    WHERE id = ?id;";


        // Add the parameters to the SQL query
        carId = Sql.Update(carQuery, args =>
        {
            args.Add("?id", id);
            args.Add("?make", make);
            args.Add("?model", model);
            args.Add("?fk_vechicle_type", bodyTypeId); // Assuming you have a foreign key for body type
            args.Add("?year", year);
            args.Add("?fk_fuel_type", fuelTypeId); // Assuming you have a foreign key for fuel type
            args.Add("?milage", mileage);
            args.Add("?color", color);
            args.Add("?technical_inspection", inspection);
            args.Add("?fk_drive_types", driveWheelsId); // Assuming you have a foreign key for drive wheels
            args.Add("?fk_gear_box", gearBox); // Assuming you have a foreign key for gearbox
            args.Add("?power", power);
            args.Add("?fk_wheel_position", steeringwheellocation); // Assuming you have a foreign key for wheel position
            args.Add("?outside_condition", outsideState);
            args.Add("?additional_functions", extraFunc);
            args.Add("?value", rating);
        });
        return carId; // Return the car ID
    }

    public static void UpdateAd(int id, string description, decimal price, bool isOrdered)
    {
        string adQuery = $@"UPDATE ad 
                    SET Description = ?Description, 
                        Price = ?Price, 
                        Ordered = ?Ordered 
                    WHERE Id = ?id;";


        // Execute the query to insert into Ad table
        Sql.Update(adQuery, args =>
        {
            args.Add("?id", id);
            args.Add("?Description", description);
            args.Add("?Price", price);
            args.Add("?Ordered", isOrdered);
            args.Add("?fk_user", 1); // Assuming fk_user is a foreign key to the user table
        });
    }

    public static void DeleteAd(int id, int carId)
    {
        string adQuery = $@"DELETE FROM car 
                    WHERE id = ?carId;";


        // Execute the query to insert into Ad table
        Sql.Delete(adQuery, args =>
        {
            args.Add("?carId", carId);

        });
        adQuery = $@"DELETE FROM ad 
                    WHERE Id = ?id;";


        // Execute the query to insert into Ad table
        Sql.Delete(adQuery, args =>
        {
            args.Add("?id", id);

        });
    }

    private static int GetFuelTypeIdFromEnum(FuelType fuelType)
    {
        // Implement the logic to map enum values to corresponding IDs from the fueltypes table
        // This could be done by querying the database or using a predefined mapping

        // Example implementation:
        switch (fuelType)
        {
            case FuelType.Diesel:
                return 1; // Assuming 1 is the ID for Gasoline in the fueltypes table
            case FuelType.DieselElectric:
                return 2; // Assuming 2 is the ID for Diesel in the fueltypes table
            case FuelType.Petrol:
                return 3; // Assuming 2 is the ID for Diesel in the fueltypes table
            case FuelType.PetrolGas:
                return 4; // Assuming 2 is the ID for Diesel in the fueltypes table
            case FuelType.PetrolElectric:
                return 5; // Assuming 2 is the ID for Diesel in the fueltypes table
            case FuelType.PetrolElectricGas:
                return 6; // Assuming 2 is the ID for Diesel in the fueltypes table
            case FuelType.Electric:
                return 7; // Assuming 2 is the ID for Diesel in the fueltypes table
            case FuelType.BioethanolE85:
                return 8; // Assuming 2 is the ID for Diesel in the fueltypes table
            case FuelType.Hydrogen:
                return 9; // Assuming 2 is the ID for Diesel in the fueltypes table
            case FuelType.Other:
                return 10; // Assuming 2 is the ID for Diesel in the fueltypes table
            default:
                throw new ArgumentException("Unknown fuel type.");
        }
    }

    private static int GetBodyTypeIdFromEnum(BodyType bodyType)
    {
        // Implement the logic to map enum values to corresponding IDs from the fueltypes table
        // This could be done by querying the database or using a predefined mapping

        // Example implementation:
        switch (bodyType)
        {
            case BodyType.Sedan:
                return 1; // Assuming 1 is the ID for Gasoline in the fueltypes table
            case BodyType.Hatchback:
                return 2; // Assuming 2 is the ID for Diesel in the fueltypes table
            case BodyType.Universal:
                return 3; // Assuming 2 is the ID for Diesel in the fueltypes table
            case BodyType.Single:
                return 4; // Assuming 2 is the ID for Diesel in the fueltypes table
            case BodyType.SUVCrossover:
                return 5; // Assuming 2 is the ID for Diesel in the fueltypes table
            case BodyType.Coupe:
                return 6; // Assuming 2 is the ID for Diesel in the fueltypes table
            case BodyType.Commercial:
                return 7; // Assuming 2 is the ID for Diesel in the fueltypes table
            case BodyType.Convertible:
                return 8; // Assuming 2 is the ID for Diesel in the fueltypes table
            case BodyType.Limousine:
                return 9; // Assuming 2 is the ID for Diesel in the fueltypes table
            case BodyType.Pickup:
                return 10; // Assuming 2 is the ID for Diesel in the fueltypes table
            case BodyType.PassengerMinibus:
                return 11; // Assuming 2 is the ID for Diesel in the fueltypes table
            case BodyType.CargoMinibus:
                return 12; // Assuming 2 is the ID for Diesel in the fueltypes table
            case BodyType.Other:
                return 13; // Assuming 2 is the ID for Diesel in the fueltypes table
            default:
                throw new ArgumentException("Unknown fuel type.");
        }
    }

    private static int GetDriveWheelsIdFromEnum(DriveWheels driveWheels)
    {
        switch (driveWheels)
        {
            case DriveWheels.Front:
                return 1; // Assuming 1 is the ID for Gasoline in the fueltypes table
            case DriveWheels.Rear:
                return 2; // Assuming 2 is the ID for Diesel in the fueltypes table
            case DriveWheels.All:
                return 3; // Assuming 2 is the ID for Diesel in the fueltypes table
            default:
                throw new ArgumentException("Unknown fuel type.");
        }
    }

    private static int GetGearboxIdFromEnum(Gearbox gearbox)
    {
        switch (gearbox)
        {
            case Gearbox.Manual:
                return 1; // Assuming 1 is the ID for Gasoline in the fueltypes table
            case Gearbox.Automatic:
                return 2; // Assuming 1 is the ID for Gasoline in the fueltypes table   
            default:
                throw new ArgumentException("Unknown fuel type.");
        }
    }

    private static int GetSteeringIdFromEnum(SteeringWheelLocation steeringWheelLocation)
    {
        switch (steeringWheelLocation)
        {
            case SteeringWheelLocation.Left:
                return 1; // Assuming 1 is the ID for Gasoline in the fueltypes table
            case SteeringWheelLocation.Right:
                return 2; // Assuming 1 is the ID for Gasoline in the fueltypes table   
            default:
                throw new ArgumentException("Unknown fuel type.");
        }
    }

    public static List<Ad> GetAllAds()
    {
        string query = @"
        SELECT a.Id, a.Description, a.Price, a.Ordered, a.Address, 
               c.id AS CarId, c.make, c.model, c.fk_vechicle_type, c.year, c.fk_fuel_type, c.milage, c.color, 
               c.technical_inspection, c.fk_drive_types, c.fk_gear_box, c.power, c.fk_wheel_position, 
               c.outside_condition, c.additional_functions, c.value
        FROM ad a
        JOIN car c ON a.Fk_Car = c.id;";

        var rows = Sql.Query(query);
        var adsWithCars = Sql.MapAll<Ad>(rows, (extractor, item) =>
        {
            item.ID= extractor.From<int>("Id");
            item.Description = extractor.From<string>("Description");
            item.Price = extractor.From<decimal>("Price");
            item.IsOrdered = extractor.From<bool>("Ordered");
            item.Address = extractor.From<string>("Address");
            item.Car = new Car
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
                Rating = Convert.ToSingle(extractor.From<decimal>("value"))
            };
            
        });

        return adsWithCars;
    }

    public static Ad GetAdAndCarById(int id)
    {
        string query = @"
        SELECT a.Id, a.Description, a.Price, a.Ordered, a.Address, 
               c.id AS CarId, c.make, c.model, c.fk_vechicle_type, c.year, c.fk_fuel_type, c.milage, c.color, 
               c.technical_inspection, c.fk_drive_types, c.fk_gear_box, c.power, c.fk_wheel_position, 
               c.outside_condition, c.additional_functions, c.value
        FROM ad a
        JOIN car c ON a.Fk_Car = c.id WHERE a.Id = ?id";

        var rows = Sql.Query(query, args =>
        {
            args.Add("?id", id);

        });
        Ad adsWithCars = Sql.MapOne<Ad>(rows, (extractor, item) =>
        {
            item.ID = extractor.From<int>("Id");
            item.Description = extractor.From<string>("Description");
            item.Price = extractor.From<decimal>("Price");
            item.IsOrdered = extractor.From<bool>("Ordered");
            item.Car = new Car
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
                Rating = Convert.ToSingle(extractor.From<decimal>("value"))
            };
            item.Address = extractor.From<string>("Address");
        });

        return adsWithCars;
    }

    public static List<SeenAd> GetSeenAds(int userId)
    {
        string query = @"
            SELECT sa.fk_ad, sa.fk_user, sa.liked
            FROM seenad sa
            WHERE sa.fk_user = ?userId;";

        var rows = Sql.Query(query, args =>
        {
            args.Add("?userId", userId);

        });
        var seenAds = Sql.MapAll<SeenAd>(rows, (extractor, item) =>
        {
            item.AdId = extractor.From<int>("fk_ad");
            item.UserId = extractor.From<int>("fk_user");
            item.liked = extractor.From<bool>("liked");
        });

        foreach(var ad in seenAds)
        {
            ad.ad = GetAdAndCarById(ad.AdId);
        }

        return seenAds;
    }

    public static List<Ad> GetLikedAds(int userId)
    {
        List<Ad> ads = new List<Ad>();
        string query = @"
            SELECT sa.fk_ad, sa.fk_user, sa.liked
            FROM seenad sa
            WHERE sa.fk_user = ?userId;";

        var rows = Sql.Query(query, args =>
        {
            args.Add("?userId", userId);

        });
        var seenAds = Sql.MapAll<SeenAd>(rows, (extractor, item) =>
        {
            item.AdId = extractor.From<int>("fk_ad");
            item.UserId = extractor.From<int>("fk_user");
            item.liked = extractor.From<bool>("liked");
        });

        foreach (var ad in seenAds)
        {
            if(ad.liked == true)
            {
                ad.ad = GetAdAndCarById(ad.AdId);
                ads.Add(ad.ad);
            }

        }

        return ads;
    }

    public static List<Car> GetUserPreferences(int userId)
    {
        string query = @"
        SELECT id AS CarId, make, model, fk_vechicle_type, year, fk_fuel_type, milage, color, 
               technical_inspection, fk_drive_types, fk_gear_box, power, fk_wheel_position, 
               outside_condition, additional_functions, value
        FROM car
        WHERE fk_user =  ?userId;";

        var rows = Sql.Query(query, args =>
        {
            args.Add("?userId", userId);

        });
        var cars = Sql.MapAll<Car>(rows, (extractor, item) =>
        {
            item.Id = extractor.From<int>("CarId");
            item.Make = extractor.From<string>("make");
            item.Model = extractor.From<string>("model");
            item.BodyType = extractor.From<BodyType>("fk_vechicle_type");
            item.Year = extractor.From<DateTime>("year");
            item.FuelType = extractor.From<FuelType>("fk_fuel_type");
            item.Mileage = extractor.From<int>("milage");
            item.Color = extractor.From<string>("color");
            item.Inspection = extractor.From<DateTime>("technical_inspection");
            item.DriveWheels = extractor.From<DriveWheels>("fk_drive_types");
            item.Gearbox = extractor.From<Gearbox>("fk_gear_box");
            item.Power = extractor.From<int>("power");
            item.SteeringWheelLocation = extractor.From<SteeringWheelLocation>("fk_wheel_position");
            item.OutsideState = extractor.From<string>("outside_condition");
            item.ExtraFunc = extractor.From<string>("additional_functions");
            item.Rating = Convert.ToSingle(extractor.From<decimal>("value"));
        });

        return cars;
    }

    public static Car GetUserPreferenceByPreferenceId(int carId)
    {
        string query = @"
        SELECT id AS CarId, make, model, fk_vechicle_type, year, fk_fuel_type, milage, color, 
               technical_inspection, fk_drive_types, fk_gear_box, power, fk_wheel_position, 
               outside_condition, additional_functions, value, fk_user
        FROM car
        WHERE id =  ?carId;";

        var rows = Sql.Query(query, args =>
        {
            args.Add("?carId", carId);

        });
        Car car = Sql.MapOne<Car>(rows, (extractor, item) =>
        {
            item.Id = extractor.From<int>("CarId");
            item.Make = extractor.From<string>("make");
            item.Model = extractor.From<string>("model");
            item.BodyType = extractor.From<BodyType>("fk_vechicle_type");
            item.Year = extractor.From<DateTime>("year");
            item.FuelType = extractor.From<FuelType>("fk_fuel_type");
            item.Mileage = extractor.From<int>("milage");
            item.Color = extractor.From<string>("color");
            item.Inspection = extractor.From<DateTime>("technical_inspection");
            item.DriveWheels = extractor.From<DriveWheels>("fk_drive_types");
            item.Gearbox = extractor.From<Gearbox>("fk_gear_box");
            item.Power = extractor.From<int>("power");
            item.SteeringWheelLocation = extractor.From<SteeringWheelLocation>("fk_wheel_position");
            item.OutsideState = extractor.From<string>("outside_condition");
            item.ExtraFunc = extractor.From<string>("additional_functions");
            item.Rating = extractor.From<int>("value");
            item.UserId = extractor.From<int>("fk_user");
        });

        return car;
    }

    public static void DeleteUserPreferenceByPreferenceId(int carId)
    {
        string adQuery = $@"DELETE FROM car 
                    WHERE id = ?carId;";


        // Execute the query to insert into Ad table
        Sql.Delete(adQuery, args =>
        {
            args.Add("?carId", carId);

        });
    }

    public static void SaveSeenAds(List<SeenAd> seenAds)
    {
        foreach (var seenAd in seenAds)
        {
            string query = @"
                INSERT INTO seenad (fk_user, fk_ad, liked)
                VALUES (?UserId, ?AdId, ?Liked);";

            Sql.Insert(query, args =>
            {
                args.Add("?UserId", seenAd.UserId);
                args.Add("?AdId", seenAd.AdId);
                args.Add("?Liked", seenAd.liked);
            });
        }
    }

    public static long InsertUserPreference(string make, string model, BodyType bodyType, DateTime year, FuelType fuelType,
     int mileage, string color, DateTime inspection, DriveWheels driveWheels, Gearbox gearbox,
     int power, SteeringWheelLocation steeringWheelLocation, string outsideState, string extraFunc, float rating, int userId)
    {
        int fuelTypeId = GetFuelTypeIdFromEnum(fuelType); // Retrieve the ID from the enum value
        int bodyTypeId = GetBodyTypeIdFromEnum(bodyType);
        int driveWheelsId = GetDriveWheelsIdFromEnum(driveWheels);
        int gearBox = GetGearboxIdFromEnum(gearbox);
        int steeringwheellocation = GetSteeringIdFromEnum(steeringWheelLocation);

        long carId = 0; // Initialize carId variable
        // Construct the SQL query for inserting into the Car table
        string carQuery = $@"INSERT INTO car (make, model, fk_vechicle_type, year, fk_fuel_type, milage, color, technical_inspection, fk_drive_types, fk_gear_box, power, fk_wheel_position, outside_condition, additional_functions, value, fk_user)                  
                        VALUES (?make, ?model, ?fk_vechicle_type, ?year, ?fk_fuel_type, ?milage, ?color, ?technical_inspection, ?fk_drive_types, ?fk_gear_box, ?power, ?fk_wheel_position, ?outside_condition, ?additional_functions, ?value, ?fk_user);
                        ";


        // Add the parameters to the SQL query
        carId = Sql.Insert(carQuery, args =>
        {
            args.Add("?make", make);
            args.Add("?model", model);
            args.Add("?fk_vechicle_type", bodyTypeId); // Assuming you have a foreign key for body type
            args.Add("?year", year);
            args.Add("?fk_fuel_type", fuelTypeId); // Assuming you have a foreign key for fuel type
            args.Add("?milage", mileage);
            args.Add("?color", color);
            args.Add("?technical_inspection", inspection);
            args.Add("?fk_drive_types", driveWheelsId); // Assuming you have a foreign key for drive wheels
            args.Add("?fk_gear_box", gearBox); // Assuming you have a foreign key for gearbox
            args.Add("?power", power);
            args.Add("?fk_wheel_position", steeringwheellocation); // Assuming you have a foreign key for wheel position
            args.Add("?outside_condition", outsideState);
            args.Add("?additional_functions", extraFunc);
            args.Add("?value", rating);
            args.Add("?fk_user", userId);
        });
        return carId; // Return the car ID
    }


}
