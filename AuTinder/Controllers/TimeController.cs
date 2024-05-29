using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using AuTinder.Models;
using AuTinder.Repositories;
using System.ComponentModel;

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


        public async Task<int> GetAverageTime(Order order)
        {
            List<User> drivers = UserRepo.GetAllDrivers();
            Dictionary<int, (double distance, int duration)> distancesAndDurations = new Dictionary<int, (double distance, int duration)>();
            List<User> PremiumDrivers = CreateEmptyPremiumList();
            int AverageTime = 0;
            Console.WriteLine(drivers.Count());
            foreach (User driver in drivers)
            {
                Console.WriteLine(driver.Name);
                (double distance, int duration) = await _mapController.GetDistanceAndDuration(driver.Address, order.Ad.Address);
                Console.WriteLine("Trip duration = " + duration);
                duration += order.Delivery.Duration;
                Console.WriteLine("Delivery duration += " + duration);
                distance = order.Delivery.Length;
                duration = AddTimeEvery10h(duration);
                Console.WriteLine("Time Every 10h += " + duration);
                duration = AddTimeBeforeTrip(driver,duration);
                Console.WriteLine("Time before trip += " + duration);
                if (order.OrderType == OrderType.Premium)
                {
                    if(duration < order.AverageTime)
                    {
                        PremiumDrivers.Add(driver);
                    }
                }

                distancesAndDurations.Add(driver.Id, (distance, duration));
            }
            if (order.OrderType == OrderType.Premium)
            {
               AverageTime = CalculateAverageTimeFromDriverList(PremiumDrivers, distancesAndDurations);
            }
            else
            {
               AverageTime = CalculateAverageTimeFromDriverList(drivers, distancesAndDurations);
            }
                

            return AverageTime;
        }

        public int CalculateAverageTimeFromDriverList(List<User> drivers, Dictionary<int, (double distance, int duration)> distancesAndDurations)
        {
            int averagetime = 0;
            int count = drivers.Count();
            foreach (User driver in drivers)
            {
                averagetime += distancesAndDurations[driver.Id].duration;
            }
            averagetime = averagetime / count;

            return averagetime;
        }

        public int AddTimeBeforeTrip(User driver, int duration)
        {
            DateTime startDateTime = DateTime.Now;
            DateTime endDateTime = driver.Date;

            if (startDateTime > endDateTime)
            {
                return duration;
            }

            TimeSpan timeDifference = endDateTime - startDateTime;
            double hoursDifference = timeDifference.TotalHours;
            return duration + (int)hoursDifference;
        }

        public int AddTimeEvery10h(int duration)
        {
            int rest_times = (int)duration / 9;
            for (int i = 0; i < rest_times; i++)
            {
                duration += 10;
            }
            return duration;
        }

        public List<User> CreateEmptyPremiumList()
        {
            return new List<User>();
        }
    }
}
