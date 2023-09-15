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
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    public class BaseController : Controller
    {        
        protected async void Init(string photo)
        {
            ViewData["Photo"] = photo;
        }
    }
}
