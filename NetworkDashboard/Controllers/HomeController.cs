using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using NetworkDashboard.Models;
using System.Diagnostics;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using NetworkDashboard.classes;

namespace NetworkDashboard.Controllers
{
    [Authorize]    
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GraphServiceClient _graphServiceClient;

        public HomeController(GraphServiceClient graphServiceClient, ILogger<HomeController> logger)
        {
            _graphServiceClient = graphServiceClient;
            _logger = logger;            
        }

        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Get user photo
                using (var photoStream = await _graphServiceClient.Me.Photo.Content.Request().GetAsync())
                {
                    byte[] photoByte = ((MemoryStream)photoStream).ToArray();
                    Init(Convert.ToBase64String(photoByte));
                }
            }
            catch (Exception pex)
            {
                Console.WriteLine($"{pex.Message}");
                
            }            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }        

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}