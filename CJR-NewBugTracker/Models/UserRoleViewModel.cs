using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJR_NewBugTracker.Models
{
    public class UserRoleViewModel
    {
        public ApplicationUser User { get; set; }
        public List<string> Role { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public MultiSelectList UserRoles { get; set; }
        public string[] SelectedRoles { get; set; }
    }
}