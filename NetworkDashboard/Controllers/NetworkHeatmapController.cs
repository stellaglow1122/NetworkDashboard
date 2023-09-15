using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetworkDashboard.Models;
using NetworkDashboard.services;
using System.Diagnostics;

namespace NetworkDashboard.Controllers
{
    [Authorize]
    public class NetworkHeatmapController : Controller
    {
        private PowerBiServiceApi powerBiServiceApi;

        public NetworkHeatmapController(PowerBiServiceApi powerBiServiceApi)
        {
            this.powerBiServiceApi = powerBiServiceApi;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Embed()
        {
            Guid workspaceId = new Guid("");
            Guid reportId = new Guid("");
            var viewModel = await powerBiServiceApi.GetReport(workspaceId, reportId);
            return View(viewModel);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
