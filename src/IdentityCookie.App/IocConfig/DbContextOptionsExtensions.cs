using System;
using System.IO;
using IdentityCookie.DataLayer.Context;
using IdentityCookie.Services;
using IdentityCookie.ViewModels.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityCookie.App.IocConfig
{
    public static class DbContextOptionsExtensions
    {
        /// <summary>
        /// Add connectionString and setup setting database
        /// </summary>
        public static IServiceCollection AddConfiguredDbContext(this IServiceCollection services, SiteSettings siteSettings)
        {
            services.AddDbContext<AppDbContext>(optionsAction => {
                optionsAction.UseSqlServer(siteSettings.ConnectionStrings.DefaultConnection
                .Replace("|DataDirectory|", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "app_data")),
                sqlServerDbContextOptionsBuilderAction => 
                {
                    var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                    sqlServerDbContextOptionsBuilderAction.CommandTimeout(minutes);
                    sqlServerDbContextOptionsBuilderAction.EnableRetryOnFailure();
                });
            });

            return services;
        }

        /// <summary>
        /// Creates and seeds the database.
        /// </summary>
        public static void InitializeDb(this IServiceProvider serviceProvider)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var identityDbInitialize = scope.ServiceProvider.GetRequiredService<IDbInitializerService>();
                identityDbInitialize.Initialize();
                identityDbInitialize.SeedData();
            }
        }
    }
}
