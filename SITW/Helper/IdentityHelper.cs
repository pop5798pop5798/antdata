using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace SITW.Helper
{
    public static class IdentityHelper
    {
        public static string GetEmail(this IIdentity identity)
        {
            var claimIdent = identity as ClaimsIdentity;
            return claimIdent != null
                && claimIdent.HasClaim(c => c.Type == "Email")
                ? claimIdent.FindFirst("Email").Value
                : string.Empty;
        }
        public static bool GetEmailConfirmed(this IIdentity identity)
        {
            var claimIdent = identity as ClaimsIdentity;
            return claimIdent != null
                && claimIdent.HasClaim(c => c.Type == "EmailConfirmed")
                ? bool.Parse(claimIdent.FindFirst("EmailConfirmed").Value)
                : false;
        }
        public static bool GetPhoneNumberConfirmed(this IIdentity identity)
        {
            var claimIdent = identity as ClaimsIdentity;
            return claimIdent != null
                && claimIdent.HasClaim(c => c.Type == "PhoneNumberConfirmed")
                ? bool.Parse(claimIdent.FindFirst("PhoneNumberConfirmed").Value)
                : false;
        }
    }
}