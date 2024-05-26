
using AuTinder.Models;
using AuTinder.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AuTinder.Controllers
{
    public class AdController : Controller
    {
        public AdController() { }
        public List<Ad> GetAds()
        {
            List<Ad> ads = AdRepo.GetAllAds();
            return ads;
        }

        public Ad GetAd(int id)
        {
            Ad ad = AdRepo.GetAdAndCarById(id);
            return ad;
        }

        public bool CheckAdData(Ad ad) 
        {
            return true;
        }

        public bool ConfirmAdStatus(Ad ad)
        {
            if (ad.IsOrdered == false)
            {
                ad.IsOrdered = true;
                AdRepo.UpdateAd(ad.ID, ad.Description, ad.Price, ad.IsOrdered);
                return false;
            }
            return true;

        }

        public bool ChangeAdStatus(Ad ad)
        {
            if(ad.IsOrdered == true)
            {
                ad.IsOrdered = false;
                AdRepo.UpdateAd(ad.ID, ad.Description, ad.Price, ad.IsOrdered);
                return true;
            }
            return true;
        }

        public void UpdateAddInfo(Ad ad)
        {
            AdRepo.UpdateCarAndAd(
                ad.ID,
                ad.Car.Id,
                ad.Car.Make,
                ad.Car.Model,
                ad.Car.BodyType,
                ad.Car.Year,
                ad.Car.FuelType,
                ad.Car.Mileage,
                ad.Car.Color,
                ad.Car.Inspection,
                ad.Car.DriveWheels,
                ad.Car.Gearbox,
                ad.Car.Power,
                ad.Car.SteeringWheelLocation,
                ad.Car.OutsideState,
                ad.Car.ExtraFunc,
                ad.Car.Rating,
                ad.Description,
                ad.Price,
                ad.IsOrdered
            );
        }

        public IActionResult DeleteAd(int id)
        {
            Ad ad = AdRepo.GetAdAndCarById(id);

            AdRepo.DeleteAd(id, ad.Car.Id);
            return RedirectToAction("ShowAdList", "Route");
        }

        


        // POST: Ad/AdCreation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Store(Ad ad)
        {
            bool good = Validate();
            if (ModelState.IsValid && good)
            {
                try
                {
                    // Call the static method from the class name
                    AdRepo.InsertCarAndAd(
                        ad.Car.Make,
                        ad.Car.Model,
                        ad.Car.BodyType,
                        ad.Car.Year,
                        ad.Car.FuelType,
                        ad.Car.Mileage,
                        ad.Car.Color,
                        ad.Car.Inspection,
                        ad.Car.DriveWheels,
                        ad.Car.Gearbox,
                        ad.Car.Power,
                        ad.Car.SteeringWheelLocation,
                        ad.Car.OutsideState,
                        ad.Car.ExtraFunc,
                        ad.Car.Rating,
                        ad.Description,
                        ad.Price,
                        ad.IsOrdered
                    );

                    TempData["Message"] = "Ad and car successfully added!";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"Error adding ad and car: {ex.Message}";
                }
            }

            return RedirectToAction("ShowAdList", "Route");
        }

        private bool Validate()
        {
            return true;
        }
        //public IActionResult Index()
        //{
        //    Route route = new Route(_logger);
        //    string s = route.randomString();
        //    return View();
        //}

        public IActionResult GetRecomendedAds()
        {
            List<Ad> ads = AdRepo.GetAllAds();
            List <SeenAd> seenAds = AdRepo.GetSeenAds(1);
            List<Car> userPreferences = AdRepo.GetUserPreferences(1);
            List<Ad> unSeenAds = FilterAds(ads, seenAds);
            List<Ad> recomendedAds = new List<Ad>();
            EvalueateAds(userPreferences, unSeenAds, recomendedAds, seenAds);
            string adsJson = JsonConvert.SerializeObject(recomendedAds);

            // Store the JSON string in TempData
            TempData["Ads"] = adsJson;
            return RedirectToAction("OpenMainView", "Route");
        }

        public List<Ad> FilterAds(List<Ad> ads, List<SeenAd> seenAds)
        {
            List<Ad> newlist = new List<Ad>();
            foreach (Ad ad in ads)
            {
                bool seen = false;
                foreach(SeenAd seenAd in seenAds)
                {
                    if (seenAd.AdId == ad.ID)
                    {
                        seen = true;
                        break;
                    }
                        
                }
                if (!seen)
                {
                    newlist.Add(ad);
                }

            }
            return newlist;
        }

        public void EvalueateAds(List<Car> userPreferences, List<Ad> unSeenAds, List<Ad> recomendedAds, List<SeenAd> seenAds)
        {
            int n = unSeenAds.Count;
            while (recomendedAds.Count < 10 && n > 0)
            {
                Ad ad = unSeenAds[n - 1];
                bool evalueation = EvalueateAdByUserPreference(ad, userPreferences);
                if (evalueation)
                {
                    recomendedAds.Add(ad);
                }
                else if (seenAds.Count != 0)
                {
                    evalueation = EvalueateAdByUserHistory(ad, seenAds);
                    if (evalueation)
                    {
                        recomendedAds.Add(ad);
                    }
                }
                n--;
            }
        }

        public bool EvalueateAdByUserPreference(Ad ad, List<Car> userPreferences)
        {
            int score = 0;
            foreach (Car pref in userPreferences)
            {
                score = score + ad.Car.CompareCars(pref);
            }
            if (score >= 3)
            {
                return true;
            }
            return false;
        }

        public bool EvalueateAdByUserHistory(Ad ad, List<SeenAd> seenAds)
        {
            List<int> repeatsLiked = new List<int>();
            List<int> repeatsDisliked = new List<int>();
            List<int> repeatsTotal = new List<int>();
            double score = 0;
            for (int i = 0; i < 16; i++)
            {
                repeatsLiked.Add(0);
                repeatsDisliked.Add(0);
                repeatsTotal.Add(0);

            }
            foreach (SeenAd seenAd in seenAds)
            {
                if (seenAd.liked == true)
                {
                    seenAd.ad.Car.CompareRpeatsInCars(ad.Car, repeatsLiked);
                    if (Math.Abs(seenAd.ad.Price - ad.Price) <= 200)
                        repeatsLiked[15]++;
                }
                else if (seenAd.liked == false)
                {
                    seenAd.ad.Car.CompareRpeatsInCars(ad.Car, repeatsDisliked);
                    if (Math.Abs(seenAd.ad.Price - ad.Price) <= 200)
                        repeatsDisliked[15]++;
                }
                seenAd.ad.Car.CompareRpeatsInCars(ad.Car, repeatsTotal);
                if (Math.Abs(seenAd.ad.Price - ad.Price) <= 200)
                    repeatsTotal[15]++;
                for (int i = 0; i < 16; i++)
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

        [HttpPost]
        public IActionResult SaveSeenAds([FromBody] List<SeenAd> seenAds)
        {
            if (seenAds.Any())
            {
                AdRepo.SaveSeenAds(seenAds);
            }

            return Ok(); // Return a successful response
        }
    }
}
