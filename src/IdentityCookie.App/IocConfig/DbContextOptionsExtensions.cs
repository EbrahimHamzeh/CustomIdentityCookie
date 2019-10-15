using System;
using System.IO;
using IdentityCookie.DataLayer.Context;
using IdentityCookie.ViewModels.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityCookie.App.IocConfig
{
    public static class DbContextOptionsExtensions
    {
        public static IServiceCollection AddConfiguredDbContext(this IServiceCollection services, SiteSettings siteSettings)
        {
            services.AddScoped<IUnitOfWork, AppDbContext>();

            services.AddDbContext<AppDbContext>(optionsAction => {
                optionsAction.UseSqlServer(siteSettings.ConnectionStrings.DefaultConnection
                .Replace("|DataDirectory|", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "app_data")),
                sqlServerDbContextOptionsBuilderAction => 
                {
                    var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                    sqlServerDbContextOptionsBuilderAction.CommandTimeout(minutes);
                    sqlServerDbContextOptionsBuilderAction.EnableRetryOnFailure(3);
                });
            });

            return services;
        }
    }
}
