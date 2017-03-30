using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJR_NewBugTracker.Models
{
    public class Project
    {
        //Table Field Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description {get; set;}


        //Child Construtor
       public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            this.User = new HashSet<ApplicationUser>();
        }
       
        //Child Navigation
        public virtual ICollection<Ticket> Tickets { get; set; }

        //Many to Many Example

       public virtual ICollection< ApplicationUser> User { get; set; }


    }
}