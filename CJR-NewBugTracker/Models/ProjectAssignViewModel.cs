using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJR_NewBugTracker.Models
{
    public class ProjectAssignViewModel
    {
        public Project project { get; set; }
        public MultiSelectList Users { get; set; }
        public string[] SelectUsers { get; set; }
    
    }
}