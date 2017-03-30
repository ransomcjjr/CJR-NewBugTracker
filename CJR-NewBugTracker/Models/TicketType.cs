using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJR_NewBugTracker.Models
{
    public class TicketType
    {
        //Properties
        public int Id { get; set; }
        public string Name { get; set; }

        //Constructor
       public TicketType()
        {
            this.Tickets = new HashSet<Ticket>();
        }

        //Child Nav
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}