﻿using System;
using System.Collections.Generic;
using IdentityCookie.DomainClasses.AuditableEntity;

namespace IdentityCookie.DomainClasses
{
    public class Role : BaseEntity, IAuditableEntity
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
