using System;
using System.Collections.Generic;
using IdentityCookie.DomainClasses.AuditableEntity;

namespace IdentityCookie.DomainClasses
{
    public class UserRole : IAuditableEntity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
