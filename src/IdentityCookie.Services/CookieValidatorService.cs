using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace IdentityCookie.Services
{
    public interface ICookieValidatorService
    {
        Task ValidateAsync(CookieValidatePrincipalContext context);
    }

    public class CookieValidatorService : ICookieValidatorService
    {
        private readonly IUsersService _usersService;

        public CookieValidatorService(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;

            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if (claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
            {
                await handleUnauthorizedRequest(context);
                return;
            }

            var serialNumberClaim = claimsIdentity.FindFirst(ClaimTypes.SerialNumber);
            if (serialNumberClaim == null)
            {
                await handleUnauthorizedRequest(context);
                return;
            }

            var userIdString = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                // this is not our issued cookie
                await handleUnauthorizedRequest(context);
                return;
            }

            var user = await _usersService.FindUserAsync(userId).ConfigureAwait(false);
            if (user == null || user.SerialNumber != serialNumberClaim.Value || !user.IsActive)
            {
                // user has changed his/her password/roles/stat/IsActive
                await handleUnauthorizedRequest(context);
            }

            await _usersService.UpdateUserLastActivityDateAsync(userId).ConfigureAwait(false);
        }

        private Task handleUnauthorizedRequest(CookieValidatePrincipalContext context)
        {
            context.RejectPrincipal();
            return context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
