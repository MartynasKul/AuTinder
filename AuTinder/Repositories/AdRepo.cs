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
        int carId = InsertCar(make, model, bodyType, year, fuelType, mileage, color, inspection, driveWheels, gearbox,
                              power, steeringWheelLocation, outsideState, extraFunc, rating);

        InsertAd(description, price, isOrdered, carId);
    }

    private static int InsertCar(string make, string model, BodyType bodyType, DateTime year, FuelType fuelType,
     int mileage, string color, DateTime inspection, DriveWheels driveWheels, Gearbox gearbox,
     int power, SteeringWheelLocation steeringWheelLocation, string outsideState, string extraFunc, float rating)
    {
        int carId = 0; // Initialize carId variable
        // Construct the SQL query for inserting into the Car table
        string carQuery = @"INSERT INTO car (make, model, fk_vechicle_type, year, fk_fuel_type, milage, color, technical_inspection, fk_drive_types, fk_gear_box, power, fk_wheel_position, outside_condition, additional_functions, value)                  
                        VALUES (@make, @model, @fk_vechicle_type, @year, @fk_fuel_type, @milage, @color, @technical_inspection, @fk_drive_types, @fk_gear_box, @power, @fk_wheel_position, @outside_condition, @additional_functions, @value);
                        SELECT SCOPE_IDENTITY()";


        // Add the parameters to the SQL query
        Sql.Insertt(carQuery, args =>
        {
            args.Add("?make", make);
            args.Add("?model", model);
            args.Add("?fk_vechicle_type", bodyType); // Assuming you have a foreign key for body type
            args.Add("?year", year);
            args.Add("?fk_fuel_type", fuelType); // Assuming you have a foreign key for fuel type
            args.Add("?milage", mileage);
            args.Add("?color", color);
            args.Add("?technical_inspection", inspection);
            args.Add("?fk_drive_types", driveWheels); // Assuming you have a foreign key for drive wheels
            args.Add("?fk_gear_box", gearbox); // Assuming you have a foreign key for gearbox
            args.Add("?power", power);
            args.Add("?fk_wheel_position", steeringWheelLocation); // Assuming you have a foreign key for wheel position
            args.Add("?outside_condition", outsideState);
            args.Add("?additional_functions", extraFunc);
            args.Add("?value", rating);
        },reader =>
        {
            if (reader.Read())
            {
                carId = Convert.ToInt32(reader[0]);
            }
        });
        return carId; // Return the car ID
    }

    private static void InsertAd(string description, decimal price, bool isOrdered, int carId)
    {
        string adQuery = @"INSERT INTO ad (Description, Price, Ordered, Fk_Car, fk_user)
                           VALUES (@Description, @Price, @Ordered, @Fk_Car, @fk_user);";

        // Execute the query to insert into Ad table
        Sql.Insert(adQuery, args =>
        {
            args.Add("@Description", description);
            args.Add("@Price", price);
            args.Add("@Ordered", isOrdered);
            args.Add("@Fk_Car", carId);
            args.Add("@fk_user", 1); // Assuming fk_user is a foreign key to the user table
        });
    }
}
