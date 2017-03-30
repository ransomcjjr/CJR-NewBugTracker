using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJR_NewBugTracker.Models
{
    public class TicketComment
    {
        //Table Field Properties
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }

        // virtual application user feed link
        public virtual ApplicationUser User { get; set; }

        //Parent Relationship
        public virtual Ticket Ticket { get; set; }

    }
}