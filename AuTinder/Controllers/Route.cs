using AuTinder.Models;
using AuTinder.Repositories;
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

        public Route(ILogger<Route> logger)
        {
            _adController = new AdController();
            _orderController = new OrderController();
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

        public IActionResult LikedAd(int id)
        {
            Ad ad = AdRepo.GetAdAndCarById(id);
            return View(ad);
        }

        public IActionResult OpenOrderForm(int id)
        {
            Ad ad = AdRepo.GetAdAndCarById(id);
            if(_adController.ConfirmAdStatus(ad) == false)
            {
                return ShowOrderForm(ad);
            }
            return RedirectToAction("ShowLikedAdList", "Route");
        }

        public IActionResult ShowOrderForm(Ad ad)
        {
           Order order = _orderController.StartOrder(ad);
            return DisplayOrderForm(order);
        }

        public IActionResult CancelOrder(int id)
        {
            Ad ad = _adController.GetAd(id);
            if (_adController.ChangeAdStatus(ad))
            {
                return RedirectToAction("ShowLikedAdList", "Route");
            }
            else
            {
                return RedirectToAction("ShowLikedAdList", "Route");
            }
        }

        public IActionResult CreateOrder(int id, OrderType type)
        {
            Order order = new Order();
            Ad ad = AdRepo.GetAdAndCarById(id);
            order.Ad = ad;
            order.OrderStatus = OrderStatus.PendingPayment;
            order.OrderType = type;
            order.Date = DateTime.Now;
            OrderRepo.CreateOrder(order);
            return RedirectToAction("ShowLikedAdList", "Route");
        }

        public IActionResult DisplayOrderForm(Order order)
        {
            return View("CreateOrder", order);
        }
        public IActionResult MakeOrderPremium(int id)
        {

            Ad ad = AdRepo.GetAdAndCarById(id);
            Order order_new = _orderController.MakeOrderPremium(ad);
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
            List<Ad> ads = _adController.GetAds();
            Console.WriteLine(1);
            return View("LikedAdLIst", ads);
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

    }
}
