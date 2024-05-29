using AuTinder.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuTinder.Controllers
{
    public class DriverController : Controller
    {
        public List<User> GetDriverList()
        {
            return UserRepo.GetDrivers();
        }
    }
}
