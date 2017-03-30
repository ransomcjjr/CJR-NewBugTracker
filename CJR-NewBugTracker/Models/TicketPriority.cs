using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJR_NewBugTracker.Models
{
    public class TicketPriority
    {
        //Properties
        public int Id { get; set; }
        public string Name { get; set; }

        //Constructor
       public TicketPriority()
        {
            this.Tickets = new HashSet<Ticket>();
        }

        //Child Nav
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}