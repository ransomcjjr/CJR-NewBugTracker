using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJR_NewBugTracker.Models
{
    public class AdminUserViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Role { get; set; }
    }
}