using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AuTinder.Controllers
{
    public class MapController : Controller
    {
        private readonly HttpClient _httpClient;

        public MapController()
        {
            _httpClient = new HttpClient();
        }


        public async Task<double> GetDistance(string address_from, string address_to)
        {
            // Replace 'YOUR_API_KEY' with your actual API key
            string apiKey = "AIzaSyBhXXLqtEuPoqNOWZZ4OqkNq4ptNREb2Zs";

            string origin = address_from;
            string destination = address_to;

            string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={origin}&destination={destination}&key={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(json);

                // Extract distance information
                int distanceInMeters = (int)data["routes"][0]["legs"][0]["distance"]["value"];

                // Convert distance to double (assuming distance is in meters)
                double distanceInKilometers = distanceInMeters / 1000.0;

                return distanceInKilometers;
            }
            else
            {
                // Handle error
                throw new Exception("Failed to retrieve distance from Google Maps API.");
            }
        }

        public async Task<(double distance, int durationHours)> GetDistanceAndDuration(string address_from, string address_to)
        {
            // Replace 'YOUR_API_KEY' with your actual API key
            string apiKey = "AIzaSyBhXXLqtEuPoqNOWZZ4OqkNq4ptNREb2Zs";
            string origin = address_from;
            string destination = address_to;

            string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={origin}&destination={destination}&key={apiKey}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(json);

                // Extract distance and duration information
                int distanceInMeters = (int)data["routes"][0]["legs"][0]["distance"]["value"];
                int durationInSeconds = (int)data["routes"][0]["legs"][0]["duration"]["value"];

                // Convert distance to kilometers
                double distanceInKilometers = distanceInMeters / 1000.0;

                // Convert duration to hours and round to the nearest integer
                int durationHours = (int)Math.Round(durationInSeconds / 3600.0);

                return (distanceInKilometers, durationHours);
            }
            else
            {
                // Handle error
                throw new Exception("Failed to retrieve distance and duration from Google Maps API.");
            }
        }
    }
}
