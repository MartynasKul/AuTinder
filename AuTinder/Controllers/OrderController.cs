using AuTinder.Models;
using AuTinder.Repositories;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using PayPal.Api;
using PayPalCheckoutSdk.Orders;
using Newtonsoft.Json;
using Mysqlx.Crud;
namespace AuTinder.Controllers
{
    public class OrderController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly MapController _mapController;
        private readonly TimeController _timeController;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
            _mapController = new MapController();
            _timeController = new TimeController();
        }

        public IActionResult Index()
        {
            return View();
        }

        public List<AuTinder.Models.Order> GetOrderList()
        {
            List<AuTinder.Models.Order> orders = OrderRepo.GetOrders();
            return orders;
        }

        [HttpGet]
        public IActionResult FilterOrders(OrderStatus orderStatus)
        {
            var orders = OrderRepo.SelectOrders(orderStatus);
            return View("~/Views/Route/OrderList.cshtml", orders);
        }

        public AuTinder.Models.Order GetOrder(int id)
        {
            // Call the method of OrderRepository to get order details
            AuTinder.Models.Order order = OrderRepo.GetOrder(id);
            return order;
        }

        public ActionResult ShowOrderDetails(int id)
        {
            var orderDetails = GetOrder(id);

            // Pass the order details to the view or handle them accordingly
            return View("~/Views/Route/ShowOrderDetails.cshtml", orderDetails);
        }

        public AuTinder.Models.Order StartOrder(Ad ad)
        {
            AuTinder.Models.Order order = new AuTinder.Models.Order();
            order.Ad = ad;
            order.OrderType = OrderType.Simple;
            order.Date = DateTime.Now;
            order.OrderStatus = OrderStatus.PendingPayment;
            Delivery delivery = new Delivery();
            delivery.Status = Delivery.DeliveryStatus.WaitingForDriver;
            delivery.AddressFrom = ad.Address;
            delivery.AddressTo = "User adress here hehe";
            AuTinder.Models.Payment payment = new AuTinder.Models.Payment();
            payment.Paid = false;
            payment.Date = DateTime.Now;

            User user = UserRepo.GetUserById(1);

            
            (double distance, int duration )= _mapController.GetDistanceAndDuration(ad.Address, user.Address).Result;
            int dis = Convert.ToInt32(distance);
            delivery.Duration = duration;
            delivery.Length = dis;
            delivery.AddressFrom = ad.Address;
            delivery.AddressTo = user.Address;
            order.Delivery = delivery;
            order.Payment = payment;
            order.AverageTime = _timeController.GetAverageTime(order).Result;
            Console.WriteLine(order.AverageTime);

            decimal price = AddDriversPrice(ad.Price, delivery.Length, order.OrderType);
            order.Price = price;
            return order;
        }

        public AuTinder.Models.Order MakeOrderPremium(AuTinder.Models.Order order)
        {
           if(order.OrderType == OrderType.Premium)
            {
                return order;
            }
            order.OrderType = OrderType.Premium;
            order.AverageTime = _timeController.GetAverageTime(order).Result;
            decimal price = AddDriversPrice(order.Ad.Price, order.Delivery.Length, order.OrderType);
            order.Price = price;
            return order;
        }

        public bool CheckType(OrderType type)
        {
            if (type != OrderType.Premium)
            {
                return true;
            }
            return false;
        }

        private decimal AddDriversPrice(decimal price, decimal distance, OrderType type)
        {
            if(CheckType(type))
            {
                price = price + distance * 10;
            }
            else
            {
                price = price + distance * 15;
            }

            return price;
        }


        public IActionResult AddOrder()
        {
            
            AuTinder.Models.Order order = new AuTinder.Models.Order();
            if (TempData["Order"] != null)
            {
                var orderJson = TempData["Order"].ToString();
                order = JsonConvert.DeserializeObject<AuTinder.Models.Order>(orderJson);
                AdRepo.UpdateAd(order.Ad.ID, order.Ad.Description, order.Ad.Price, order.Ad.IsOrdered);
                order = OrderRepo.CreateOrder(order);
                order.Id = OrderRepo.GetLastInsertedOrderId();
                TempData["Order"] = JsonConvert.SerializeObject(order);
            }
            string currency = "EUR";
            int user = 1; 

            var config = new Dictionary<string, string>
            {
                { "mode", "sandbox" }, // "live" or "sandbox"
                { "clientId", "AeAQCuup2vFmac9Duayh8zWsKW5-Fx5l-K4HGk2xs96mDMTrKEi82J3keD0mlAmprppu8EmwtMJ3TGyQ" },
                { "clientSecret", "EDc1EAhLcN6GSdaheOfbsVU-MXxyP_bIMixhMqTq2_dmKum1ChmPF9eBTXu0-36dRSbMm9IR4nUgE6EJ" }
            };

            var accessToken = new OAuthTokenCredential("AeAQCuup2vFmac9Duayh8zWsKW5-Fx5l-K4HGk2xs96mDMTrKEi82J3keD0mlAmprppu8EmwtMJ3TGyQ", "EDc1EAhLcN6GSdaheOfbsVU-MXxyP_bIMixhMqTq2_dmKum1ChmPF9eBTXu0-36dRSbMm9IR4nUgE6EJ", config).GetAccessToken();
            var apiContext = new APIContext(accessToken);


            var payment = new PayPal.Api.Payment
            {
                intent = "sale",
                payer = new PayPal.Api.Payer
                {
                    payment_method = "paypal"
                },
                transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        description = "Order payment",
                        invoice_number = order.Id.ToString(),
                        amount = new Amount
                        {   currency = currency,
                            total = order.Price.ToString("F2",CultureInfo.InvariantCulture)
                        },
                         item_list = new ItemList
                        {
                            items = new List<PayPal.Api.Item>
                            {
                                new PayPal.Api.Item
                                {
                                    name = "Rattata",
                                    currency = currency,
                                    price = order.Price.ToString("F2", CultureInfo.InvariantCulture),
                                    quantity = 1.ToString(),
                                    sku = order.Id.ToString("F2", CultureInfo.InvariantCulture)
                                }
                            }
                        }
                    }
                },
                redirect_urls = new RedirectUrls
                {
                    return_url = Url.Action("PaymentDone", "Order", null, Request.Scheme),
                    cancel_url = Url.Action("CancelPayment", "Order", null, Request.Scheme)
                }
            };

            // Create PayPal payment
            var createdPayment = payment.Create(apiContext);

            // Retrieve approval URL
            var approvalUrl = createdPayment.links.FirstOrDefault(link => link.rel == "approval_url")?.href;
            Console.WriteLine(approvalUrl);
            if (approvalUrl != null)
            {
                return Redirect(approvalUrl);
            }
            else
            {
                if (TempData["Order"] != null)
                {
                    var orderJson = TempData["Order"].ToString();
                    order = JsonConvert.DeserializeObject<AuTinder.Models.Order>(orderJson);
                    order.OrderStatus = OrderStatus.Cancelled;
                    OrderRepo.UpdateOrder(order.Id, order);
                }
                order.Ad.IsOrdered = false;
                AdRepo.UpdateAd(order.Ad.ID, order.Ad.Description, order.Ad.Price, order.Ad.IsOrdered);

                TempData["OrderResponse"] = JsonConvert.SerializeObject("Payment failed, order canceled");
                TempData.Remove("Order");
                return RedirectToAction("ShowLikedAdList", "Route"); ;
            }
        }

        public IActionResult PaymentDone()
        {
            AuTinder.Models.Order order = new AuTinder.Models.Order();
            if (TempData["Order"] != null)
            {
                var orderJson = TempData["Order"].ToString();
                order = JsonConvert.DeserializeObject<AuTinder.Models.Order>(orderJson);
            }
            TempData["OrderResponse"] = JsonConvert.SerializeObject("Order was complete, you can find your new order in your oder list :D");
            order.OrderStatus = OrderStatus.Paid;
            order.Payment.Paid = true;
            OrderRepo.UpdatePayment(order.Payment.Id, order.Payment);
            order.OrderStatus = OrderStatus.Paid;
            OrderRepo.UpdateOrder(order.Id, order);
            order.Ad.IsOrdered = true;
            AdRepo.UpdateAd(order.Ad.ID, order.Ad.Description, order.Ad.Price, order.Ad.IsOrdered);
            TempData.Remove("Order");
            return RedirectToAction("ShowLikedAdList", "Route");
        }
        
        /*
        public IActionResult ExecutePayment(string paymentId, string PayerID)
        {
            Console.WriteLine("Do i even go here xd");
            AuTinder.Models.Order order = new AuTinder.Models.Order();
            if (TempData["Order"] != null)
            {
                var orderJson = TempData["Order"].ToString();
                order = JsonConvert.DeserializeObject<AuTinder.Models.Order>(orderJson);
            }

            var config = new Dictionary<string, string>
            {
                { "mode", "sandbox" }, // "live" or "sandbox"
                { "clientId", "AeAQCuup2vFmac9Duayh8zWsKW5-Fx5l-K4HGk2xs96mDMTrKEi82J3keD0mlAmprppu8EmwtMJ3TGyQ" },
                { "clientSecret", "EDc1EAhLcN6GSdaheOfbsVU-MXxyP_bIMixhMqTq2_dmKum1ChmPF9eBTXu0-36dRSbMm9IR4nUgE6EJ" }
            };

            var accessToken = new OAuthTokenCredential("AeAQCuup2vFmac9Duayh8zWsKW5-Fx5l-K4HGk2xs96mDMTrKEi82J3keD0mlAmprppu8EmwtMJ3TGyQ", "EDc1EAhLcN6GSdaheOfbsVU-MXxyP_bIMixhMqTq2_dmKum1ChmPF9eBTXu0-36dRSbMm9IR4nUgE6EJ", config).GetAccessToken();
            var apiContext = new APIContext(accessToken);

            var paymentExecution = new PaymentExecution
            {
                payer_id = PayerID
            };
            var payment = new PayPal.Api.Payment { id = paymentId };

            Console.WriteLine($"Executing payment with ID: {paymentId} and PayerID: {PayerID}");

            var executedPayment = payment.Execute(apiContext, paymentExecution);
            if (executedPayment.state.ToLower() == "approved")
            {
                TempData["Message"] = JsonConvert.SerializeObject(1);
                order.OrderStatus = OrderStatus.Paid;
                order.Payment.Paid = true;
                Console.WriteLine("we got here");
                OrderRepo.UpdatePayment(order.Payment.Id, order.Payment);
            }
            else
            {
                TempData["Message"] = JsonConvert.SerializeObject(0);
            }

            return RedirectToAction("Route", "LikedAdList");

        }*/

        public IActionResult CancelPayment()
        {
            AuTinder.Models.Order order = new AuTinder.Models.Order();
            if (TempData["Order"] != null)
            {
                var orderJson = TempData["Order"].ToString();
                order = JsonConvert.DeserializeObject<AuTinder.Models.Order>(orderJson);
                order.OrderStatus = OrderStatus.Cancelled;
                OrderRepo.UpdateOrder(order.Id, order);
            }
            order.Ad.IsOrdered = false;
            AdRepo.UpdateAd(order.Ad.ID, order.Ad.Description, order.Ad.Price, order.Ad.IsOrdered);

            TempData["OrderResponse"] = JsonConvert.SerializeObject("Payment failed, order canceled");
            TempData.Remove("Order");
            return RedirectToAction("ShowLikedAdList", "Route");
        }

    }
}
