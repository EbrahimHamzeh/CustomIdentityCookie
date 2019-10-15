using System;
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

            
            services.AddConfiguredDbContext(siteSettings);
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
