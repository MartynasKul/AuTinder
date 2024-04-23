using AuTinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using System.Diagnostics;

namespace AuTinder.Controllers
{
    public class Route : Controller
    {
        private readonly ILogger<Route> _logger;

        public Route(ILogger<Route> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AdCreation() 
        {
            return View();
        }
        public IActionResult Ad(Ad ad) 
        {
            return View(ad);
        }
        public IActionResult AdDelete(Ad ad) 
        {
            return View(ad);
        }
        public IActionResult AdList() 
        {
            List<Ad> ads = new List<Ad>
            {
                new Ad
                {
                    Description = "Economical and reliable car",
                    Price = 5000m,
                    IsOrdered = false,
                    Car = new Car
                    {
                        Make = "Toyota",
                        Model = "Corolla",
                        BodyType = "Sedan",
                        Year = new DateTime(2018, 1, 1),
                        Mileage = 50000,
                        FuelType = "Gas",
                        Color = "Blue",
                        Inspection = new DateTime(2022, 12, 1),
                        DriveWheels = "FWD",
                        Gearbox = "Automatic",
                        Power = 132,
                        SteeringWheelLocation = "Left",
                        OutsideState = "Good",
                        ExtraFunc = "Air Conditioning",
                        Rating = 4.5f
                    }
                },
                new Ad
                {
                    Description = "Luxury sport coupe",
                    Price = 27000m,
                    IsOrdered = false,
                    Car = new Car
                    {
                        Make = "BMW",
                        Model = "4 Series",
                        BodyType = "Coupe",
                        Year = new DateTime(2016, 1, 1),
                        Mileage = 34000,
                        FuelType = "Petrol",
                        Color = "Red",
                        Inspection = new DateTime(2023, 1, 10),
                        DriveWheels = "RWD",
                        Gearbox = "Manual",
                        Power = 300,
                        SteeringWheelLocation = "Left",
                        OutsideState = "Excellent",
                        ExtraFunc = "Heated Seats",
                        Rating = 4.8f
                    }
                },
                new Ad
                {
                    Description = "Family SUV with excellent safety features",
                    Price = 22000m,
                    IsOrdered = true,
                    Car = new Car
                    {
                        Make = "Honda",
                        Model = "CR-V",
                        BodyType = "SUV",
                        Year = new DateTime(2020, 1, 1),
                        Mileage = 15000,
                        FuelType = "Hybrid",
                        Color = "Silver",
                        Inspection = new DateTime(2024, 1, 5),
                        DriveWheels = "AWD",
                        Gearbox = "Automatic",
                        Power = 212,
                        SteeringWheelLocation = "Left",
                        OutsideState = "Very Good",
                        ExtraFunc = "Collision Avoidance System",
                        Rating = 4.7f
                    }
                },
                new Ad
                {
                    Description = "Affordable compact car, great for city driving",
                    Price = 8000m,
                    IsOrdered = false,
                    Car = new Car
                    {
                        Make = "Ford",
                        Model = "Fiesta",
                        BodyType = "Hatchback",
                        Year = new DateTime(2017, 1, 1),
                        Mileage = 60000,
                        FuelType = "Diesel",
                        Color = "Green",
                        Inspection = new DateTime(2023, 2, 15),
                        DriveWheels = "FWD",
                        Gearbox = "Automatic",
                        Power = 85,
                        SteeringWheelLocation = "Left",
                        OutsideState = "Good",
                        ExtraFunc = "Auto Start/Stop",
                        Rating = 4.2f
                    }
                },
                new Ad
                {
                    Description = "High-performance sports car, ready for the track",
                    Price = 55000m,
                    IsOrdered = true,
                    Car = new Car
                    {
                        Make = "Porsche",
                        Model = "718 Cayman",
                        BodyType = "Coupe",
                        Year = new DateTime(2019, 1, 1),
                        Mileage = 25000,
                        FuelType = "Petrol",
                        Color = "Black",
                        Inspection = new DateTime(2023, 3, 30),
                        DriveWheels = "RWD",
                        Gearbox = "Manual",
                        Power = 350,
                        SteeringWheelLocation = "Left",
                        OutsideState = "Like New",
                        ExtraFunc = "Sport Chrono Package",
                        Rating = 4.9f
                    }
                }
            };
            return View(ads);
        }
        public IActionResult AdEdit(Ad ad) 
        {

            if (ModelState.IsValid) 
            {
                //SaveAd(ad);
                return RedirectToAction("Index");
            }
            return View(ad);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
