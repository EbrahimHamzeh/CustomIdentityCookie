using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace IdentityCookie.App.Area.Admin.Controllers
{
    [Area(AreaConstants.Admin)]
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;

        public TaskController(ILogger<TaskController> logger)
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
