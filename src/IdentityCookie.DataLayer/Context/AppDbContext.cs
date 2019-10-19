using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using IdentityCookie.DomainClasses;
using IdentityCookie.DomainClasses.AuditableEntity;
using IdentityCookie.Common;

namespace IdentityCookie.DataLayer.Context
{
    public class AppDbContext : DbContext, IUnitOfWork
    {
        private IDbContextTransaction _transaction;
        private readonly ISecurityService _securityService;

        public AppDbContext(DbContextOptions<AppDbContext> options, ISecurityService securityService)
         : base(options)
        {
            _securityService = securityService;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<AppLogItem> AppLogItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(builder);

            builder.Entity<User>(entityAction =>
            {
                entityAction.Property(e => e.Username).HasMaxLength(450).IsRequired();
                entityAction.HasIndex(e => e.Username).IsUnique();
                entityAction.Property(e => e.Password).IsRequired();
                entityAction.Property(e => e.SerialNumber).HasMaxLength(450);
            });

            builder.Entity<Role>(entityAction =>
            {
                // entityAction.Property(e => e.Name).IsRequired().IsUnicode();
                // entityAction.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<UserRole>(entityAction =>
            {
                entityAction.HasKey(e => new { e.UserId, e.RoleId });
                entityAction.HasIndex(e => e.UserId);
                entityAction.HasIndex(e => e.RoleId);

                entityAction.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
                entityAction.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId);
            });

            // TODO: نتونستم ذخیره کنم برای همین seed جدا مینوسم.
            // var adminRole = new Role { Id = Guid.Parse("c5d28c9e-7d13-4fd3-bb47-8cab0dca9ade"), Name = "Admin" };
            // var user = new User
            // {
            //     Id = Guid.Parse("0b5f8384-40c0-499f-b16f-34361b08c460"),
            //     Username = "admin",
            //     DisplayName = "ابراهیم حمزه",
            //     IsActive = true,
            //     LastLoggedIn = null,
            //     Password = _securityService.GetSha256Hash("123"),
            //     SerialNumber = Guid.NewGuid().ToString("N"),
            // };

            // builder.Entity<Role>().HasData(adminRole);

            // builder.Entity<User>().HasData(user);

            // builder.Entity<UserRole>().HasData(new UserRole { RoleId = adminRole.Id, Role = adminRole, UserId = user.Id, User = user });

            // This should be placed here, at the end.
            builder.AddAuditableShadowProperties();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            // optionsBuilder.EnableSensitiveDataLogging(); // Beter log exception
        }

        #region Defult Function
        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().AddRange(entities);
        }

        public void BeginTransaction()
        {
            _transaction = Database.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            if (_transaction == null)
            {
                throw new NullReferenceException("Please call `BeginTransaction()` method first.");
            }
            _transaction.Rollback();
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw new NullReferenceException("Please call `BeginTransaction()` method first.");
            }
            _transaction.Commit();
        }

        public override void Dispose()
        {
            _transaction?.Dispose();
            base.Dispose();
        }

        public void ExecuteSqlInterpolatedCommand(FormattableString query)
        {
            Database.ExecuteSqlInterpolated(query);
        }

        public void ExecuteSqlRawCommand(string query, params object[] parameters)
        {
            Database.ExecuteSqlRaw(query, parameters);
        }

        public T GetShadowPropertyValue<T>(object entity, string propertyName) where T : IConvertible
        {
            var value = this.Entry(entity).Property(propertyName).CurrentValue;
            return value != null
                ? (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture)
                : default;
        }

        public object GetShadowPropertyValue(object entity, string propertyName)
        {
            return this.Entry(entity).Property(propertyName).CurrentValue;
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Update(entity);
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().RemoveRange(entities);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges(); //NOTE: changeTracker.Entries<T>() will call it automatically.

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChanges();
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChangesAsync(cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        private void beforeSaveTriggers()
        {
            validateEntities();
            setShadowProperties();
        }

        private void setShadowProperties()
        {
            // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
            var httpContextAccessor = this.GetService<IHttpContextAccessor>();
            ChangeTracker.SetAuditableEntityPropertyValues(httpContextAccessor);
        }

        private void validateEntities()
        {
            var errors = this.GetValidationErrors();
            if (!string.IsNullOrWhiteSpace(errors))
            {
                // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
                var loggerFactory = this.GetService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<AppDbContext>();
                logger.LogError(errors);
                throw new InvalidOperationException(errors);
            }
        }
        #endregion
    }

}