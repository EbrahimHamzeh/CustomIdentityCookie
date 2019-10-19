using System;
using System.Threading.Tasks;
using IdentityCookie.Common;
using IdentityCookie.DataLayer.Context;
using IdentityCookie.DomainClasses;
using Microsoft.EntityFrameworkCore;

namespace IdentityCookie.Services
{
    public interface IUsersService
    {
        Task<string> GetSerialNumberAsync(int userId);
        Task<User> FindUserAsync(string username, string password);
        Task<User> FindUserAsync(int userId);
        Task UpdateUserLastActivityDateAsync(int userId);
    }
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<User> _users;
        private readonly ISecurityService _securityService;

        public UsersService(IUnitOfWork uow, ISecurityService securityService)
        {
            _uow = uow;
            _users = _uow.Set<User>();
            _securityService = securityService;
        }

        public async Task<User> FindUserAsync(int userId)
        {
            return await _users.FindAsync(userId);
        }

        public async Task<User> FindUserAsync(string username, string password)
        {
            var passwordHash = _securityService.GetSha256Hash(password);
            return await _users.FirstOrDefaultAsync(x => x.Username == username && x.Password == passwordHash);
        }

        public async Task<string> GetSerialNumberAsync(int userId)
        {
            var user = await FindUserAsync(userId).ConfigureAwait(false);
            return user.SerialNumber;
        }

        public async Task UpdateUserLastActivityDateAsync(int userId)
        {
            var user = await FindUserAsync(userId).ConfigureAwait(false);
            if (user.LastLoggedIn != null)
            {
                var updateLastActivityDate = TimeSpan.FromMinutes(2);
                var currentUtc = DateTimeOffset.UtcNow;
                var timeElapsed = currentUtc.Subtract(user.LastLoggedIn.Value);
                if (timeElapsed < updateLastActivityDate)
                {
                    return;
                }
            }
            user.LastLoggedIn = DateTimeOffset.UtcNow;
            await _uow.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
