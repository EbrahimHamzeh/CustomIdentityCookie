using System;
using IdentityCookie.Common;
using IdentityCookie.DataLayer.Context;
using IdentityCookie.Services;
using IdentityCookie.ViewModels.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IdentityCookie.App.IocConfig
{
    public static class ServicesRegistry
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            var siteSettings = GetSiteSettings(services);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork, AppDbContext>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IDbInitializerService, DbInitializerService>();
            services.AddScoped<ICookieValidatorService, CookieValidatorService>();
            
            services.AddConfiguredDbContext(siteSettings);
            services.AddConfiguredIdentity(siteSettings);
        }

        public static SiteSettings GetSiteSettings(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var siteSettingsOptions = provider.GetRequiredService<IOptionsSnapshot<SiteSettings>>();
            var siteSettings = siteSettingsOptions.Value;
            if(siteSettings == null) throw new ArgumentNullException(nameof(siteSettings));
            return siteSettings;
        }
    }
}
