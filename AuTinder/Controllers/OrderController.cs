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
    }
}
