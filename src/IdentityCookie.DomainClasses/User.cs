using System;
using System.Collections.Generic;

namespace IdentityCookie.DomainClasses
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset? LastLoggedIn { get; set; }
        public string SerialNumber { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
