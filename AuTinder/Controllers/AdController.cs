
using AuTinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
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
    }
}
