using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace IdentityCookie.App.Area.Admin.Controllers
{
    [Area(AreaConstants.Admin)]
    public class LetterCardController : Controller
    {
        private readonly ILogger<LetterCardController> _logger;

        public LetterCardController(ILogger<LetterCardController> logger)
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
