using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJR_NewBugTracker.Models
{
    public class ProjectUserRoleVM
    {
        public Project Project;
        public List<ApplicationUser> Users;
        public List<string> Roles;
    }
}