using System;
using System.Linq;
using IdentityCookie.Common;
using IdentityCookie.DataLayer.Context;
using IdentityCookie.DomainClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityCookie.Services
{
    public interface IDbInitializerService
    {
        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Adds some default values to the Db
        /// </summary>
        void SeedData();
    }

    public class DbInitializerService : IDbInitializerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ISecurityService _securityService;

        public DbInitializerService(IServiceScopeFactory scopeFactory, ISecurityService securityService)
        {
            _scopeFactory = scopeFactory;
            _securityService = securityService;
        }

        public void Initialize()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<AppDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        public void SeedData()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<AppDbContext>())
                {
                    var adminRole = new Role { Id = Guid.NewGuid(), Name = "Admin" };
                    if (!context.Roles.Any())
                    {
                        context.Add(adminRole);
                        context.SaveChanges();
                    }

                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        Username = "admin",
                        DisplayName = "ابراهیم حمزه",
                        IsActive = true,
                        LastLoggedIn = null,
                        Password = _securityService.GetSha256Hash("123"),
                        SerialNumber = Guid.NewGuid().ToString("N"),
                    };

                    if (!context.Users.Any())
                    {
                        context.Add(user);
                        context.SaveChanges();

                        if (!context.UserRoles.Any())
                        {
                            context.Add(new UserRole { Role = adminRole, User = user });
                            context.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
