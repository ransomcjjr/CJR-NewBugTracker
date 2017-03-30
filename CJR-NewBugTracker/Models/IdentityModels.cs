using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CJR_NewBugTracker.Models;

namespace CJR_NewBugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
        public ApplicationUser()
        {
            this.Projects = new HashSet<Project>();
            this.TicketAttachements = new HashSet<TicketAttachment>();
            this.TicketComments = new HashSet<TicketComment>();
            this.TicketHistories = new HashSet<TicketHistory>();
            this.TicketNotificaitons = new HashSet<TicketNotification>();
        }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<TicketAttachment> TicketAttachements { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotificaitons { get; set; }


          public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("Name",FullName));
            return userIdentity;
        }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //Add all Database Tables/Models Here
        public DbSet<Ticket> Tickets {get; set;}
        public DbSet<TicketStatus>TicketStatuses{get;set;}
        public DbSet <TicketAttachment>TicketAttachments{ get; set; }
        public DbSet<TicketHistory> TicketHistories{ get; set; }
        public DbSet<TicketComment> TicketComments{ get; set; }
        public DbSet<TicketNotification> TicketNotifications{ get; set; }
        public DbSet<TicketPriority>TicketProrities{ get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Project> Projects{ get; set; }

    }
}
