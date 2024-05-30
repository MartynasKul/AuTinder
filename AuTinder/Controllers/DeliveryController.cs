using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.OpenPgp;
using AuTinder.Models;
using AuTinder.Repositories;
using K4os.Compression.LZ4.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using Org.BouncyCastle.Bcpg;

namespace AuTinder.Controllers
{
    public class DeliveryController : Controller
    {
        public DeliveryController() { }
        public List<Delivery> GetDeliveries()
        {
            List<Delivery> deliveries = DeliveryRepo.GetAllDeliveries();
            return deliveries;
        }

        public Delivery GetDelivery(int id)
        {
            Delivery delivery = DeliveryRepo.GetDeliveryAndOrderById(id);
            return delivery;
        }

        public IActionResult GetRecomendedDeliveries()
        {
            List<Delivery> deliveries = DeliveryRepo.GetAllDeliveries();
            List<SeenDelivery> seenDeliveries = DeliveryRepo.GetSeenDeliveries(2);
            List<Delivery> userPreferences = DeliveryRepo.GetUserPreferences(2);
            List<Delivery> unSeenDeliveries = FilterDeliveries(deliveries, seenDeliveries);
            List<Delivery> recomendedDeliveries = new List<Delivery>();
            EvalueateDeliveries(userPreferences, unSeenDeliveries, recomendedDeliveries, seenDeliveries);
            string deliveriesJson = JsonConvert.SerializeObject(recomendedDeliveries);

            // Store the JSON string in TempData
            TempData["Deliveries"] = deliveriesJson;
            return RedirectToAction("OpenMainView", "Route");
        }

        public List<Delivery> FilterDeliveries(List<Delivery> deliveries, List<SeenDelivery> seenDeliveries)
        {
            List<Delivery> newlist = new List<Delivery>();
            foreach (Delivery delivery in deliveries)
            {
                bool seen = false;
                foreach (SeenDelivery seenDelivery in seenDeliveries)
                {
                    if (seenDelivery.DeliveryId == delivery.Id)
                    {
                        seen = true;
                        break;
                    }

                }
                if (!seen)
                {
                    newlist.Add(delivery);
                }

            }
            return newlist;
        }

        public void EvalueateDeliveries(List<Delivery> userPreferences, List<Delivery> unSeenDeliveries, List<Delivery> recomendedDeliveries, List<SeenDelivery> seenDeliveries)
        {
            int n = unSeenDeliveries.Count;
            while (recomendedDeliveries.Count < 10 && n > 0)
            {
                Delivery delivery = unSeenDeliveries[n - 1];
                bool evalueation = EvalueateDeliveryByUserPreference(delivery, userPreferences);
                if (evalueation)
                {
                    recomendedDeliveries.Add(delivery);
                }
                else if (seenDeliveries.Count != 0)
                {
                    evalueation = EvalueateDeliveryByUserHistory(delivery, seenDeliveries);
                    if (evalueation)
                    {
                        recomendedDeliveries.Add(delivery);
                    }
                }
                n--;
            }
        }

        public bool EvalueateDeliveryByUserPreference(Delivery delivery, List<Delivery> userPreferences)
        {
            int score = 0;
            foreach (Delivery pref in userPreferences)
            {
                score = score + delivery.CompareDeliveries(pref);
            }
            if (score >= 3)
            {
                return true;
            }
            return false;
        }

        public bool EvalueateDeliveryByUserHistory(Delivery delivery, List<SeenDelivery> seenDeliveries)
        {
            List<int> repeatsLiked = new List<int>();
            List<int> repeatsDisliked = new List<int>();
            List<int> repeatsTotal = new List<int>();
            double score = 0;
            for (int i = 0; i < 3; i++)
            {
                repeatsLiked.Add(0);
                repeatsDisliked.Add(0);
                repeatsTotal.Add(0);

            }
            foreach (SeenDelivery seenDelivery in seenDeliveries)
            {
                if (seenDelivery.liked == true)
                {
                    seenDelivery.Delivery.CompareRpeatsInDeliveries(delivery, repeatsLiked);
                }
                else if (seenDelivery.liked == false)
                {
                    seenDelivery.Delivery.CompareRpeatsInDeliveries(delivery, repeatsDisliked);
                }
                seenDelivery.Delivery.CompareRpeatsInDeliveries(delivery, repeatsTotal);
                for (int i = 0; i < 3; i++)
                {
                    if (repeatsTotal[i] > 0)
        {
                        score = score + (repeatsLiked[i] / repeatsTotal[i]);
                        score = score - (repeatsDisliked[i] / repeatsTotal[i]);
        }


                }

            }
            if (score > 0)
                return true;
            return false;
        }

        public List<SeenDelivery> ShowLikedDeliveries()
        {
            List<SeenDelivery> del = DeliveryRepo.GetLikedDeliveries(1);
            return del;
        }

        [HttpPost]
        public IActionResult SaveSeenDeliveries([FromBody] List<SeenDelivery> seenDeliveries)
        {
            if (seenDeliveries.Any())
            {
                DeliveryRepo.SaveSeenDeliveries(seenDeliveries);
            }

            return Ok(); // Return a successful response
        }

        public IActionResult AddDeliveryToRoute(int id)
        {
            DeliveryRoute route = DeliveryRepo.GetRouteForFinalDelivery(2);
            foreach (Delivery delivery in route.Deliveries)
            {
                if (delivery.Id == id)
                {
                    return RedirectToAction("GetLikedDeliveries", "Route");
                }
            }
            DeliveryRepo.AddDeliveyToRoute(id, 2, 2);
            return RedirectToAction("GetLikedDeliveries", "Route");
        }

        //public IActionResult StartFinalDelivery()
        //{
        //    int userid = 2;
        //    DeliveryRoute route = DeliveryRepo.GetRouteForFinalDelivery(userid);
        //    return View("FinalDeliveryView", "Route");
        //}


        public IActionResult RemoveFromRoute(int deliveryID) 
        {
            DeliveryRepo.RemoveDeliveryFromRoute(deliveryID);

            return RedirectToAction("StartFinalDelivery", "Route");
        }
    }
}
