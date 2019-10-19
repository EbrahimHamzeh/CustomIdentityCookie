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
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUsersService _userService;
        private readonly IRolesService _rolesService;
        private readonly IOptionsSnapshot<SiteSettings> _siteSettings;

        public LoginController(ILogger<LoginController> logger, IUsersService userService, IRolesService rolesService, IOptionsSnapshot<SiteSettings> siteSettings)
        {
            _logger = logger;
            _userService = userService;
            _rolesService = rolesService;
            _siteSettings = siteSettings;
        }

        [AllowAnonymous, HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.FindUserAsync(model.Username, model.Password);
                if (user == null || !user.IsActive)
                {
                    ModelState.AddModelError("", "نام کاربری و یا رمزعبور معتبر نمی باشد.");
                    return View(model);
                }

                var cookieClaims = await createCookieClaimsAsync(user);

                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                cookieClaims,
                new AuthenticationProperties
                {
                    IsPersistent = true, // "Remember Me"
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(_siteSettings.Value.LoginCookieExpirationDays)
                });
                await _userService.UpdateUserLastActivityDateAsync(user.Id).ConfigureAwait(false);
            }

            return View(model);
        }

        public async Task<bool> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return true;
        }


        private async Task<ClaimsPrincipal> createCookieClaimsAsync(User user)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            identity.AddClaim(new Claim("DisplayName", user.DisplayName));

            // to invalidate the cookie
            identity.AddClaim(new Claim(ClaimTypes.SerialNumber, user.SerialNumber));

            // custom data
            identity.AddClaim(new Claim(ClaimTypes.UserData, user.Id.ToString()));

            // add roles
            var roles = await _rolesService.FindUserRolesAsync(user.Id).ConfigureAwait(false);
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
            }

            return new ClaimsPrincipal(identity);
        }

        public bool IsAuthenthenticated()
        {
            return User.Identity.IsAuthenticated;
        }

        public IActionResult GetUserInfo()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            return Json(new { Username = claimsIdentity.Name });
        }
    }
}
