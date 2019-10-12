using System;
using System.Collections.Generic;

namespace IdentityCookie.DomainClasses
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
