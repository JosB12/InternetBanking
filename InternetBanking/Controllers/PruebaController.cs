using Microsoft.AspNetCore.Mvc;

namespace InternetBanking.Controllers
{
    public class PruebaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
