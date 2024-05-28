using AuTinder.Models;
using AuTinder.Repositories;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using PayPal.Api;
using PayPalCheckoutSdk.Orders;
using Newtonsoft.Json;
namespace AuTinder.Controllers
{
    public class OrderController : Controller
    {

        private readonly IConfiguration _configuration;


        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
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
            delivery.DeliveryStatus = DeliveryStatus.WaitingForDriver;
            delivery.Address_from = ad.Address;
            delivery.Address_to = "User adress here hehe";
            AuTinder.Models.Payment payment = new AuTinder.Models.Payment();
            payment.Paid = false;
            payment.Date = DateTime.Now;

            //Calculating distance
            delivery.Duration = 10;
            delivery.Length = 10;
            order.Delivery = delivery;
            order.Payment = payment;
            decimal distance = 1;


            decimal price = AddDriversPrice(ad.Price, distance, order.OrderType);
            order.Price = price;
            return order;
        }

        public AuTinder.Models.Order MakeOrderPremium(AuTinder.Models.Order order)
        {
           
            order.OrderType = OrderType.Premium;

            //Calculating distance
            decimal distance = 1;
            decimal price = AddDriversPrice(order.Ad.Price, distance, order.OrderType);
            order.Price = price;
            return order;
        }

        private decimal AddDriversPrice(decimal price, decimal distance, OrderType type)
        {
            if(type == OrderType.Simple)
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
                OrderRepo.CreateOrder(order);

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

            Console.WriteLine(accessToken);
            Console.WriteLine(apiContext.ToString());



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
                    return_url = "https://localhost:7293/Route/ShowLikedAdList?fromProfile=true",
                    cancel_url = "https://localhost:7293/Route/ShowLikedAdList?fromProfile=true"
                }
            };

            // Create PayPal payment
            var createdPayment = payment.Create(apiContext);

            // Retrieve approval URL
            var approvalUrl = createdPayment.links.FirstOrDefault(link => link.rel == "approval_url")?.href;
            Console.WriteLine(approvalUrl);
            if (approvalUrl != null)
            {
                // Redirect user to PayPal for payment approval
                Console.WriteLine("Ok how");
                return Redirect(approvalUrl);
            }
            else
            {
                Console.WriteLine("IDFC");
                // Failure: Set order status to "Failed" and insert into repository
                return RedirectToAction("Route", "LikedAdList");
            }
        }

        public IActionResult ExecutePayment(string paymentId, string PayerID)
        {
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

            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);

            var paymentExecution = new PaymentExecution
            {
                payer_id = PayerID
            };
            var payment = new PayPal.Api.Payment { id = paymentId };
            var executedPayment = payment.Execute(apiContext, paymentExecution);

            if (executedPayment.state.ToLower() == "approved")
            {
                //success
                order.OrderStatus = OrderStatus.Paid;
               
            }
            else
            {
                //failure=
            }

            return RedirectToAction("Route", "LikedAdList");

        }

        public IActionResult CancelPayment()
        {

            return RedirectToAction("Route", "LikedAdList");
        }

    }
}
