
using AuTinder.Models;
using AuTinder.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Cryptography.Pkcs;

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

        public List<Ad> GetLikedAds()
        {
            List<Ad> ads = AdRepo.GetLikedAds(1);
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
            CalculateScore(ad);
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
            List<Car> userPreferences = AdRepo.GetUserPreferences(1);
            List <SeenAd> seenAds = AdRepo.GetSeenAds(1);
            List<Ad> unSeenAds = FilterSeenAds(ads, seenAds);
            List<Ad> recomendedAds = new List<Ad>();
            EvalueateAds(userPreferences, unSeenAds, recomendedAds, seenAds);
            if (recomendedAds.Count == 0)
            {
                TempData["NoAds"] = "No ads available right now, try later";
                return RedirectToAction("OpenMainView", "Route");
            }
            string adsJson = JsonConvert.SerializeObject(recomendedAds);

            // Store the JSON string in TempData
            TempData["Ads"] = adsJson;
            return RedirectToAction("OpenMainView", "Route");
        }

        public List<Ad> FilterSeenAds(List<Ad> ads, List<SeenAd> seenAds)
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



        #region Automobilio_ivercio_skaiciavimas

        /// <summary>
        /// funny score counting method
        /// </summary>
        /// <returns></returns>
        public void CalculateScore(Ad ad)
        {
            User user = UserRepo.GetUserById(1);
            double score = 0;

            int orderCount = ReadHistory(user);

            if (orderCount > 5)
            {
                score = AddPoints(5, score);
            }
            else
            {
                score = AddPoints(1, score);
            }

            DateTime caryear = GetCarYear(ad.Car);
            DateTime cutoff = new DateTime(1990, 1, 1);

            if (caryear > new DateTime(1990, 1, 1))
            {
                score = AddPoints(((caryear.Year - cutoff.Year) / 5), score);
            }
            else
            {
                score = AddPoints(1, score);
            }

            DateTime ta = GetTaInfo(ad.Car);

            if (ta > DateTime.Today)
            {
                score = AddPoints(1, score);
            }

            string features = GetCarSafetyFeatures(ad.Car);
            string[] feat = features.Split(',');

            if (feat.Length > 0)
            {
                score = AddPoints(feat.Length, score);
            }

            if (GetCarPower(ad.Car) > 200)
            {
                score = AddPoints(3, score);
            }
            else if (GetCarPower(ad.Car) > 100 && GetCarPower(ad.Car) <= 200)
            {
                score = AddPoints(2, score);
            }
            else
            {
                score = AddPoints(1, score);
            }

            string description = GetDescription(ad);

            string result = CompareDescriptionWithHardCodedStatements(description);
            string[] res = result.Split(",");

            if (res[0] == "More positive")
            {
                int pointsToAdd = Convert.ToInt32((string)res[1]);
                score = AddPoints(pointsToAdd, score);
            }
            else if (res[0] == "More negative")
            {
                int pointsToAdd = Convert.ToInt32((string)res[1]);
                score = DecreasePoints(pointsToAdd, score);
            }
            else
            {
                // nieko nedaro nes nesugalvota ig
            }

            score = AverageOutPoints(6, score);


            ad.Car.Rating = Convert.ToSingle(score);
        }

        // Predefined lists of positive and negative words
        private static readonly HashSet<string> PositiveWords = new HashSet<string>
        {
            "excellent", "great", "amazing", "fantastic", "good", "reliable", "comfortable",
            "perfect", "wonderful", "affordable", "low-mileage", "clean", "like-new", "well-maintained"
        };

        private static readonly HashSet<string> NegativeWords = new HashSet<string>
        {
            "bad", "poor", "terrible", "broken", "damaged", "expensive", "unreliable", "uncomfortable",
            "dirty", "old", "high-mileage", "rusty", "noisy", "problematic"
        };
        public int ReadHistory(User user) 
        {
            return user.OrderCount;
        }

        public double AddPoints(int point, double currentPoints) 
        {
            return currentPoints += point;
        }
        public double DecreasePoints(int point, double currentPoints)
        {
            return currentPoints -= point;
        }
        public double AverageOutPoints(int avgamount, double score) 
        {
            return score/avgamount;
        }

        public User GetUserInfo(User user) 
        {
            return user;
        }

        public DateTime GetCarYear(Car car) 
        {
            return car.Year;
        }

        public DateTime GetTaInfo(Car car) 
        {
            return car.Inspection;
        }
        public string GetCarSafetyFeatures(Car car)
        {
            return car.ExtraFunc;
        }
        public int GetCarPower(Car car) 
        {
            return car.Power;
        }

        public string GetDescription(Ad ad) 
        {
            return ad.Description;
        }

        public static string CompareDescriptionWithHardCodedStatements(string description)
        {
            // Normalize the description to lowercase and split into words
            string[] words = description.ToLower().Split(new char[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            int positiveCount = 0;
            int negativeCount = 0;

            // Count the occurrences of positive and negative words
            foreach (string word in words)
            {
                if (PositiveWords.Contains(word))
                {
                    positiveCount++;
                }
                else if (NegativeWords.Contains(word))
                {
                    negativeCount++;
                }
            }

            // Determine which count is higher
            if (positiveCount > negativeCount)
            {
                return "More positive,"+positiveCount;
            }
            else if (negativeCount > positiveCount)
            {
                return "More negative,"+negativeCount;
            }
            else
            {
                return "Neutral";
            }
        }

        #endregion

    }
}
