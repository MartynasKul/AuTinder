using AuTinder.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
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
        string adQuery = $@"INSERT INTO ad (Description, Price, Ordered, Fk_Car, fk_user)
                           VALUES (?Description, ?Price, ?Ordered, ?Fk_Car, ?fk_user);";

        // Execute the query to insert into Ad table
        Sql.Insert(adQuery, args =>
        {
            args.Add("?Description", description);
            args.Add("?Price", price);
            args.Add("?Ordered", isOrdered);
            args.Add("?Fk_Car", carId);
            args.Add("?fk_user", 1); // Assuming fk_user is a foreign key to the user table
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
}
