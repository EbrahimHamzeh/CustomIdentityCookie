using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace IdentityCookie.App.Area.Admin.Controllers
{
    [Area(AreaConstants.Admin)]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous, HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
