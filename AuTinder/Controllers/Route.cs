using AuTinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AuTinder.Controllers
{
    public class Route : Controller
    {
        private readonly ILogger<Route> _logger;
        private AdController _adController;
        private OrderController _orderController;
        private UserController _userController;

        public Route(ILogger<Route> logger)
        {
            _adController = new AdController();
            _orderController = new OrderController();
            _userController = new UserController();
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Retrieve message from TempData
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"];
            }
            else
            {
                ViewBag.Message = ""; // Set a default value if TempData["Message"] is not found
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // GET: Ad/AdCreation
        public IActionResult ShowCreateAd()
        {
            return View("AdCreateView");
        }


        public IActionResult Ad(int id)
        {
            Ad ad = AdRepo.GetAdAndCarById(id);
            return View(ad);
        }

        public ActionResult ShowOrderDetails(int id)
        {
            var orderDetails = _orderController.GetOrder(id);

            // Pass the order details to the view or handle them accordingly
            return View(orderDetails);
        }

        public IActionResult ShowAdList()
        {
            //List<Ad> ads = new List<Ad>
            //{
            //    new Ad
            //    {
            //        Description = "Economical and reliable car",
            //        Price = 5000m,
            //        IsOrdered = false,
            //        Car = new Car
            //        {
            //            Make = "Toyota",
            //            Model = "Corolla",
            //            BodyType = BodyType.Sedan,
            //            Year = new DateTime(2018, 1, 1),
            //            Mileage = 50000,
            //            FuelType = FuelType.PetrolGas,
            //            Color = "Blue",
            //            Inspection = new DateTime(2022, 12, 1),
            //            DriveWheels = DriveWheels.Front,
            //            Gearbox = Gearbox.Automatic,
            //            Power = 132,
            //            SteeringWheelLocation = SteeringWheelLocation.Left,
            //            OutsideState = "Good",
            //            ExtraFunc = "Air Conditioning",
            //            Rating = 4.5f
            //        }
            //    },
            //    new Ad
            //    {
            //        Description = "Luxury sport coupe",
            //        Price = 27000m,
            //        IsOrdered = false,
            //        Car = new Car
            //        {
            //            Make = "BMW",
            //            Model = "4 Series",
            //            BodyType = BodyType.Coupe,
            //            Year = new DateTime(2016, 1, 1),
            //            Mileage = 34000,
            //            FuelType = FuelType.Petrol,
            //            Color = "Red",
            //            Inspection = new DateTime(2023, 1, 10),
            //            DriveWheels = DriveWheels.Rear,
            //            Gearbox = Gearbox.Manual,
            //            Power = 300,
            //            SteeringWheelLocation = SteeringWheelLocation.Left,
            //            OutsideState = "Excellent",
            //            ExtraFunc = "Heated Seats",
            //            Rating = 4.8f
            //        }
            //    },
            //    new Ad
            //    {
            //        Description = "Family SUV with excellent safety features",
            //        Price = 22000m,
            //        IsOrdered = true,
            //        Car = new Car
            //        {
            //            Make = "Honda",
            //            Model = "CR-V",
            //            BodyType = BodyType.SUVCrossover,
            //            Year = new DateTime(2020, 1, 1),
            //            Mileage = 15000,
            //            FuelType = FuelType.Hydrogen,
            //            Color = "Silver",
            //            Inspection = new DateTime(2024, 1, 5),
            //            DriveWheels = DriveWheels.Rear,
            //            Gearbox = Gearbox.Automatic,
            //            Power = 212,
            //            SteeringWheelLocation = SteeringWheelLocation.Left,
            //            OutsideState = "Very Good",
            //            ExtraFunc = "Collision Avoidance System",
            //            Rating = 4.7f
            //        }
            //    },
            //    new Ad
            //    {
            //        Description = "Affordable compact car, great for city driving",
            //        Price = 8000m,
            //        IsOrdered = false,
            //        Car = new Car
            //        {
            //            Make = "Ford",
            //            Model = "Fiesta",
            //            BodyType = BodyType.Hatchback,
            //            Year = new DateTime(2017, 1, 1),
            //            Mileage = 60000,
            //            FuelType = FuelType.Diesel,
            //            Color = "Green",
            //            Inspection = new DateTime(2023, 2, 15),
            //            DriveWheels = DriveWheels.Front,
            //            Gearbox = Gearbox.Automatic,
            //            Power = 85,
            //            SteeringWheelLocation = SteeringWheelLocation.Left,
            //            OutsideState = "Good",
            //            ExtraFunc = "Auto Start/Stop",
            //            Rating = 4.2f
            //        }
            //    },
            //    new Ad
            //    {
            //        Description = "High-performance sports car, ready for the track",
            //        Price = 55000m,
            //        IsOrdered = true,
            //        Car = new Car
            //        {
            //            Make = "Porsche",
            //            Model = "718 Cayman",
            //            BodyType = BodyType.Coupe,
            //            Year = new DateTime(2019, 1, 1),
            //            Mileage = 25000,
            //            FuelType = FuelType.Petrol,
            //            Color = "Black",
            //            Inspection = new DateTime(2023, 3, 30),
            //            DriveWheels = DriveWheels.Rear,
            //            Gearbox = Gearbox.Manual,
            //            Power = 350,
            //            SteeringWheelLocation = SteeringWheelLocation.Left,
            //            OutsideState = "Like New",
            //            ExtraFunc = "Sport Chrono Package",
            //            Rating = 4.9f
            //        }
            //    }
            //};
            //List<Ad> ads = AdRepo.GetAllAdsAndCars();

            List<Ad> ads = _adController.GetAds();
            Console.WriteLine(1);
            return View("AdLIst", ads);
        }
        public IActionResult ShowOrderList()
        {
            List<Order> orders = _orderController.GetOrderList();
            Console.WriteLine(1);
            return View("OrderList", orders);
        }

        public IActionResult ShowEditAd(int id)
        {
            Ad ad = AdRepo.GetAdAndCarById(id);
            //if (ModelState.IsValid) 
            //{
            //    //SaveAd(ad);
            //    return RedirectToAction("Index");
            //}
            return View("AdEdit", ad);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AdEdit(Ad ad)
        {
            bool DataGood = _adController.CheckAdData(ad);
            if (DataGood)
            {
                _adController.UpdateAddInfo(ad);
            }

            return RedirectToAction("ShowAdList", "Route");
        }

        public IActionResult Delete(int id)
        {
            Ad ad = AdRepo.GetAdAndCarById(id);

            AdRepo.DeleteAd(id, ad.Car.Id);
            return RedirectToAction("AdList", "Route");
        }

        public IActionResult OpenProfile()
        {
            return View("ProfileView");

        }

        public IActionResult OpenMainView()
        {
            List<Ad> ads = null;

            if (TempData.ContainsKey("Ads"))
            {
                // Retrieve the JSON string from TempData
                string adsJson = TempData["Ads"] as string;

                // Deserialize the JSON string back to a list of ads
                ads = JsonConvert.DeserializeObject<List<Ad>>(adsJson);
            }

            return View("MainView", ads);
        }

        public IActionResult ShowPreferenceView()
        {

            List<Car> cars = _userController.GetUserPreferences(1);

            return View("PreferenceView", cars);
        }

        public IActionResult EditPreference(int id)
        {
            Car car = _userController.GetUserPreference(id);
            return View("PreferenceEdit", car);
        }

        public IActionResult CreatePreference()
        {
            return View();
        }

        public IActionResult UpdatePreference(Car preference)
        {
            _userController.UpdateUserPreference(preference);
            return RedirectToAction("ShowPreferenceView");
        }


    }
}
