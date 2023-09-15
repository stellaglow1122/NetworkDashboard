using Microsoft.AspNetCore.Mvc;
using NetworkDashboard.Models;
using NuGet.Protocol;

namespace NetworkDashboard.Controllers
{
    public class HeatmapdeviceListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<string> AddNewNetworkdeviceAsync(string device, string IP, string Column7, string Column3, string Column1, string Column2, string Column4)
        {
            try
            {
                HeatmapDatabase heatmapDatabase = new HeatmapDatabase();
                return await heatmapDatabase.AddNetworkdevice(device, IP, Column7, Column3, Column1, Column2, Column4);
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
        [HttpPost]
        public async Task<string> EditNetworkdeviceAsync(string device, string IP, string Column7, string Column3, string Column1, string Column2, string Column4)
        {
            try
            {
                HeatmapDatabase heatmapDatabase = new HeatmapDatabase();
                return await heatmapDatabase.EditNetworkdevice(device, IP, Column3, Column1, Column2, Column4);
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
        [HttpPost]
        public async Task<string> DeleteNetworkdeviceAsync(string networkdeviceID)
        {
            try
            {
                HeatmapDatabase heatmapDatabase = new HeatmapDatabase();
                return await heatmapDatabase.DeleteNetworkdevice(networkdeviceID);
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
        [HttpPost]
        public async Task<string> GetNetworkdeviceAsync(string networkdeviceName)
        {
            try
            {
                HeatmapDatabase heatmapDatabase = new HeatmapDatabase();
                IEnumerable<Networkdevice> networkdeviceList = await heatmapDatabase.GetNetworkdevice(networkdeviceName);
                foreach (Networkdevice networkdevice in networkdeviceList.ToList())
                {
                    return networkdevice.ToJson();
                }
                return (new Networkdevice()).ToJson();
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }
    }
}
