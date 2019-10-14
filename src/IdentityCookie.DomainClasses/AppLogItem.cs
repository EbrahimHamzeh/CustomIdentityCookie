using System;
using System.Collections.Generic;
using IdentityCookie.DomainClasses.AuditableEntity;

namespace IdentityCookie.DomainClasses
{
    public class AppLogItem : BaseEntity, IAuditableEntity
    {
        public DateTime? CreatedDateTime { get; set; }
        
        public int EventId { get; set; }

        public string Url { get; set; }

        public string LogLevel { get; set; }

        public string Logger { get; set; }

        public string Message { get; set; }

        public string StateJson { get; set; }
    }
}
