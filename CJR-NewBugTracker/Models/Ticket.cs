using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJR_NewBugTracker.Models
{
    public class Ticket
    {
       
        //Ticket Properties (Same as table)
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int ProjectId { get; set; }
        public int TicketTypeId { get; set; }
        public int TicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }
        public string OwnerUserId { get; set; }
        public string AssignedToUserId { get; set; }

        //Ticket Constructors for Children
        public Ticket()
        {
            this.TicketAttachements = new HashSet<TicketAttachment>();
            this.TicketComments = new HashSet<TicketComment>();
            this.TicketHistories = new HashSet<TicketHistory>();
            this.TicketNotificaitons = new HashSet<TicketNotification>();
        }

        //Child Navigation
        public virtual ICollection<TicketAttachment> TicketAttachements { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotificaitons { get; set; }

        // virtual application user feed link
        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignedToUser{ get; set; }

        //Parent Relationship
        public  virtual Project Project { get; set; }
        public  virtual TicketStatus TicketStatus { get; set; }
        public  virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketType TicketType { get; set; }


    }
}