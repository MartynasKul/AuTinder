using Microsoft.AspNetCore.Mvc;

using AuTinder.Models;

namespace AuTinder.Controllers
{
    public class UserController : Controller
    {


        [HttpPost]
        public IActionResult SavePreferences([FromBody] List<Car> cars)
        {
            return Ok("Data saved successfully.");
        }

        public List<Car> GetUserPreferences(int id)
        {
            List<Car> cars = AdRepo.GetUserPreferences(id);
            return cars;
        }

        

        public Car GetUserPreference(int id)
        {
            Car car = AdRepo.GetUserPreferenceByPreferenceId(id);
            return car;
        }

        public IActionResult DeletePreference(int id)
        {
            AdRepo.DeleteUserPreferenceByPreferenceId(id);
            return Ok();

        }

        public void UpdateUserPreference(Car preference) {
            AdRepo.UpdateCar(preference.Id, preference.Make, preference.Model, preference.BodyType, preference.Year, preference.FuelType, preference.Mileage,
                preference.Color, preference.Inspection, preference.DriveWheels, preference.Gearbox, preference.Power, preference.SteeringWheelLocation, preference.OutsideState,
                preference.ExtraFunc, preference.Rating);

        }

        public IActionResult AddPreference(Car preference)
        {
            int userid = 1;
            AdRepo.InsertUserPreference(preference.Make, preference.Model, preference.BodyType, preference.Year, preference.FuelType, preference.Mileage,
                preference.Color, preference.Inspection, preference.DriveWheels, preference.Gearbox, preference.Power, preference.SteeringWheelLocation, preference.OutsideState,
                preference.ExtraFunc, preference.Rating, userid);
            return RedirectToAction("ShowPreferenceView", "Route");

        }
    }
}
