using Microsoft.AspNetCore.Mvc;

namespace NetworkDashboard.Controllers
{
    public class NetworkUpTime : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
