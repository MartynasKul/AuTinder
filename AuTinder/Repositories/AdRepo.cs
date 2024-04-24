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
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = configuration.GetConnectionString("DbConnStr");

        int carId;

        // Construct the SQL query for inserting into the Car table
        string carQuery = @"INSERT INTO car (make, model, fk_vechicle_type, year, fk_fuel_type, milage, color, technical_inspection, fk_drive_types, fk_gear_box, power, fk_wheel_position, outside_condition, additional_functions, value)                  
                        VALUES (@make, @model, @fk_vechicle_type, @year, @fk_fuel_type, @milage, @color, @technical_inspection, @fk_drive_types, @fk_gear_box, @power, @fk_wheel_position, @outside_condition, @additional_functions, @value);
                        SELECT SCOPE_IDENTITY();"; // Retrieve the ID of the newly inserted car record

        // Execute the query to insert into Car table and retrieve the ID of the newly inserted record
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(carQuery, connection))
            {
                command.Parameters.AddWithValue("@make", make);
                command.Parameters.AddWithValue("@model", model);
                command.Parameters.AddWithValue("@fk_vechicle_type", bodyType.ToString());
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@fk_fuel_type", fuelType.ToString());
                command.Parameters.AddWithValue("@milage", mileage);
                command.Parameters.AddWithValue("@color", color);
                command.Parameters.AddWithValue("@technical_inspection", inspection);
                command.Parameters.AddWithValue("@fk_drive_types", driveWheels.ToString());
                command.Parameters.AddWithValue("@fk_gear_box", gearbox.ToString());
                command.Parameters.AddWithValue("@power", power);
                command.Parameters.AddWithValue("@fk_wheel_position", steeringWheelLocation.ToString());
                command.Parameters.AddWithValue("@outside_condition", outsideState);
                command.Parameters.AddWithValue("@additional_functions", extraFunc);
                command.Parameters.AddWithValue("@value", rating);

                // Execute the query and retrieve the newly inserted car ID
                carId = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        // Construct the SQL query for inserting into the Ad table
        string adQuery = @"INSERT INTO ad (Description, Price, Ordered, Fk_Car, fk_user)
                        VALUES (@Description, @Price, @Ordered, @Fk_Car, @fk_user);";

        // Execute the query to insert into Ad table
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(adQuery, connection))
            {
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@Ordered", isOrdered);
                command.Parameters.AddWithValue("@Fk_Car", carId);
                command.Parameters.AddWithValue("@fk_user", 1); // Assuming fk_user is a foreign key to the user table

                command.ExecuteNonQuery();
            }
        }
    }
}
