using AuTinder.Models;
using AuTinder.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;

namespace AuTinder.Controllers
{
    public class Route : Controller
    {
        private readonly ILogger<Route> _logger;
        private AdController _adController;
        private OrderController _orderController;
        private UserController _userController;
        private DeliveryController _deliveryController;
        private readonly IConfiguration _configuration;
        public Route(ILogger<Route> logger)
        {
            _adController = new AdController();
            _orderController = new OrderController(_configuration);
            _userController = new UserController();
            _deliveryController = new DeliveryController();
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

        public IActionResult ShowDelivery()
        {
            Delivery delivery = null;
            if (TempData.ContainsKey("Delivery"))
            {
                // Retrieve the JSON string from TempData
                string deliveryJson = TempData["Delivery"] as string;

                // Deserialize the JSON string back to a list of deliveries
                delivery = JsonConvert.DeserializeObject<Delivery>(deliveryJson);
            }
            return View("DeliveryView", delivery);
        }

        public IActionResult Ad(int id)
        {
            Ad ad = AdRepo.GetAdAndCarById(id);
            return View(ad);
        }

        public IActionResult LikedAd(int id)
        {
            Ad ad = AdRepo.GetAdAndCarById(id);
            return View(ad);
        }

        // This action displays the details of a single liked delivery
        public IActionResult LikedDelivery(int deliveryId)
        {
            var delivery = DeliveryRepo.GetDelivery(deliveryId);
            if (delivery == null)
            {
                return NotFound();
            }

            var seenDelivery = new SeenDelivery
            {
                DeliveryId = delivery.Id,
                UserId = 1, // Replace with logic to get the current user's ID
                liked = true, // Determine if the user liked this delivery
                Delivery = delivery
            };

            return View(seenDelivery);
        }

        public IActionResult OpenOrderForm(int id)
        {
            Ad ad = AdRepo.GetAdAndCarById(id);
            if (_adController.ConfirmAdStatus(ad) == false)
            {
                ad.IsOrdered = true;
                return ShowOrderForm(ad);
            }
            return RedirectToAction("ShowLikedAdList", "Route");
        }

        public IActionResult ShowOrderForm(Ad ad)
        {
            Order order = _orderController.StartOrder(ad);
            TempData["Order"] = JsonConvert.SerializeObject(order);
            return DisplayOrderForm(order);
        }

        public IActionResult CancelOrder()
        {
            TempData.Remove("Order");
            return RedirectToAction("ShowLikedAdList", "Route");
        }

        public IActionResult DisplayOrderForm(Order order)
        {
            TempData["Address_To"] = UserRepo.GetUserById(1).Address;
            TempData["Address_From"] = order.Ad.Address;
            return View("CreateOrder", order);
        }

        public IActionResult MakeOrderPremium()
        {
            Order order_new = new Order();
            if (TempData["Order"] != null)
            {
                var orderJson = TempData["Order"].ToString();
                var order = JsonConvert.DeserializeObject<Order>(orderJson);
                order_new = _orderController.MakeOrderPremium((Order)order);
                TempData["Address_To"] = UserRepo.GetUserById(1).Address;
                TempData["Address_From"] = order_new.Ad.Address;
                TempData["Order"] = JsonConvert.SerializeObject(order_new);
            }
            return View("CreateOrder", order_new);
        }

        public ActionResult ShowOrderDetails(int id)
        {
            var orderDetails = _orderController.GetOrder(id);

            // Pass the order details to the view or handle them accordingly
            return View(orderDetails);
        }

        public IActionResult ShowAdList()
        {
            List<Ad> ads = _adController.GetAds();
            Console.WriteLine(1);
            return View("AdLIst", ads);
        }

        public IActionResult ShowLikedAdList()
        {
            List<Ad> ads = _adController.GetLikedAds();
            Console.WriteLine(1);
            return View("LikedAdLIst", ads);
        }
        public IActionResult GetLikedDeliveries() 
        {
            List<SeenDelivery> del = _deliveryController.ShowLikedDeliveries();

            return View("LikedDeliveries", del);
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
            List<Delivery> deliveries = null;

            if (TempData.ContainsKey("NoAds"))
            {
                ViewBag.NoAds = TempData["NoAds"];
            }
            if (TempData.ContainsKey("Ads"))
            {
                // Retrieve the JSON string from TempData
                string adsJson = TempData["Ads"] as string;

                // Deserialize the JSON string back to a list of ads
                ads = JsonConvert.DeserializeObject<List<Ad>>(adsJson);
            }
            else if (TempData.ContainsKey("Deliveries"))
            {
                // Retrieve the JSON string from TempData
                string deliveriesJson = TempData["Deliveries"] as string;

                // Deserialize the JSON string back to a list of ads
                deliveries = JsonConvert.DeserializeObject<List<Delivery>>(deliveriesJson);
            }

            MainViewModel mvm = new MainViewModel(ads, deliveries);

            return View("MainView", mvm);
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

        public IActionResult ShowLikedDeliveries()
        {
            return View("DeliveryRouteView");
        }

        public IActionResult StartFinalDelivery()
        {
            int userid = 2;
            DeliveryRoute route = DeliveryRepo.GetRouteForFinalDelivery(userid);
            return View("FinalDeliveryView", route.Deliveries);
        }

        public IActionResult EndRoute()
        {
            DeliveryRepo.updateDeliveryRoute(2);


            List<SeenDelivery> del = _deliveryController.ShowLikedDeliveries();

            return View("LikedDeliveries", del);
        }
    }
}
