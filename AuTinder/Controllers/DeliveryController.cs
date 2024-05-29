using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.OpenPgp;
using AuTinder.Models;
using AuTinder.Repositories;

namespace AuTinder.Controllers
{
    public class DeliveryController : Controller
    {
        public DeliveryController() { }

        public List<SeenDelivery> ShowLikedDeliveries() 
        {
            List<SeenDelivery> del = DeliveryRepo.GetLikedDeliveries(1);
            return del;
        }

        //this code has c



    }
}
