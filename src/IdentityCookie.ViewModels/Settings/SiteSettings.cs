using System;
using DNTCommon.Web.Core;

namespace IdentityCookie.ViewModels.Settings
{
    public class SiteSettings
    {
        public AdminUserSeed AdminUserSeed { get; set; }
        public Logging Logging { get; set; }
        public SmtpConfig Smtp { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public string ContentSecurityPolicyErrorLogUri { get; set; }
    }
}
