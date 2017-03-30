using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJR_NewBugTracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }

        // virtual application user feed link
        public virtual ApplicationUser User { get; set; }

        //Parent Relationship
        public virtual Ticket Ticket { get; set; }

    }

}