using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NetworkDashboard.Controllers
{
    public class deviceListController : Controller
    {
        // GET: deviceListController
        public ActionResult Index()
        {

            return View();
        }

        // GET: deviceListController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: deviceListController/Create        
    }
}
