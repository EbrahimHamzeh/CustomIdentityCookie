using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityCookie.App.Models;
using IdentityCookie.App.Area.Admin;
using IdentityCookie.Services;
using Microsoft.Extensions.Options;
using IdentityCookie.ViewModels.Settings;
using IdentityCookie.DomainClasses;
using Microsoft.AspNetCore.Authorization;
using IdentityCookie.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

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
