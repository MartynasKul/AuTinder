using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using AuTinder.Models;
using AuTinder.Repositories;

namespace AuTinder.Controllers
{
    public class TimeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly MapController _mapController;

        public TimeController()
        {
            _httpClient = new HttpClient();
            _mapController = new MapController();  
        }


        public async Task<Dictionary<int, (double distance, double duration)>> GetDistancesAndDurationsForDrivers(Order order)
        {
            List<User> drivers = UserRepo.GetAllDrivers();
            Dictionary<int, (double distance, double duration)> distancesAndDurations = new Dictionary<int, (double distance, double duration)>();

            foreach (User driver in drivers)
            {
                (double distance, int duration) = await _mapController.GetDistanceAndDuration(driver.Address, order.Ad.Address);
                duration = order.Delivery.Duration;
                distance = order.Delivery.Length;
                distancesAndDurations.Add(driver.Id, (distance, duration));
                duration = AddTimeEvery10h(duration);
            }

            return distancesAndDurations;
        }

        public int AddTimeEvery10h(int duration)
        {
            int rest_times = (int)duration / 9;
            for (int i = 0; i < rest_times; i++)
            {
                duration += 10;
            }
            return rest_times;
        }

        public List<User> CreateEmptyPremiumList()
        {
            return new List<User>();
        }
    }
}
