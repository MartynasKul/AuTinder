using AuTinder.Models;
using AuTinder.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AuTinder.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<Order> GetOrderList()
        {
            List<Order> orders = OrderRepo.GetOrders();
            return orders;
        }

        [HttpGet]
        public IActionResult FilterOrders(OrderStatus orderStatus)
        {
            var orders = OrderRepo.SelectOrders(orderStatus);
            return View("~/Views/Route/OrderList.cshtml", orders);
        }

        public Order GetOrder(int id)
        {
            // Call the method of OrderRepository to get order details
            Order order = OrderRepo.GetOrder(id);
            return order;
        }

        public ActionResult ShowOrderDetails(int id)
        {
            var orderDetails = GetOrder(id);

            // Pass the order details to the view or handle them accordingly
            return View("~/Views/Route/ShowOrderDetails.cshtml", orderDetails);
        }

        public Order StartOrder(Ad ad)
        {
            Order order = new Order();
            order.Ad = ad;
            order.OrderType = OrderType.Simple;
            order.Date = DateTime.Now;
            order.OrderStatus = OrderStatus.PendingPayment;

            //Calculating distance
            decimal distance = 1200;
            decimal price = AddDriversPrice(ad.Price, distance, order.OrderType);
            order.Price = price;
            return order;
        }

        public Order MakeOrderPremium(Order order)
        {
           
            order.OrderType = OrderType.Premium;

            //Calculating distance
            decimal distance = 1200;
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

    }
}
