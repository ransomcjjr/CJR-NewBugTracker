using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace CJR_NewBugTracker.Models.helpers
{
        public static class StatucHelper
        {
        [ValidateAntiForgeryToken]
        public static string GetFullName(this IIdentity user)
            {
                var ClaimsUser = (ClaimsIdentity)user;
                var claim = ClaimsUser.Claims.FirstOrDefault(c => c.Type == "Name");
                if (claim != null)
                {
                    return claim.Value;
                }
                else
                {
                    return null;
                }
            }
        }
    }