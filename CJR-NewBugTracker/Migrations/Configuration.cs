//namespace CJR_NewBugTracker.Migrations
//{
//    using Microsoft.AspNet.Identity;
//    using Microsoft.AspNet.Identity.EntityFramework;
//    using Models;
//    using System.Data.Entity.Migrations;
//    using System.Linq;

//    internal sealed class Configuration : DbMigrationsConfiguration<CJR_NewBugTracker.Models.ApplicationDbContext>
//    {
//        public Configuration()
//        {
//            AutomaticMigrationsEnabled = true;
//        }
//    }
//}

//protected override void Seed(CJR_NewBugTracker.Models.ApplicationDbContext context)
//{
//    var roleManager = new RoleManager<IdentityRole>(
//                    new RoleStore<IdentityRole>(context));
//    if (!context.Roles.Any(r => r.Name == "Admin"))
//    {
//        roleManager.Create(new IdentityRole { Name = "Admin" });
//    }
//    if (!context.Roles.Any(r => r.Name == "Project Manager"))
//    {
//        roleManager.Create(new IdentityRole { Name = "Project Manager" });
//    }
//    if (!context.Roles.Any(r => r.Name == "Developer"))
//    {
//        roleManager.Create(new IdentityRole { Name = "Developer" });
//    }
//    if (!context.Roles.Any(r => r.Name == "Project Manager"))
//    {
//        roleManager.Create(new IdentityRole { Name = "Project Manager" });
//    }
//    if (!context.Roles.Any(r => r.Name == "Submitter"))
//    {
//        roleManager.Create(new IdentityRole { Name = "Submitter" });
//    }

//    var userManager = new UserManager<ApplicationUser>(
//        new UserStore<ApplicationUser>(context));
//    if (!context.Users.Any(u => u.Email == "ransomcjjr@gmail.com"))
//    {
//        userManager.Create(new ApplicationUser
//        {

//            UserName = "ransomcjjr@gmail.com",
//            Email = "ransomcjjr@gmail.com",
//            EmailConfirmed = true
//        }, "change2016!");
//    }

//    var userId = userManager.FindByEmail("ransomcjjr@gmail.com").Id;
//    userManager.AddToRole(userId, "Admin");

//    //TODO: Seed Types-Priorities-Status
//    //Ticket Type Table
//    context.TicketTypes.AddOrUpdate(t => t.Id,
//    new TicketType()
//    {
//        Name = "BugReport"
//    },
//    new TicketType()
//    {
//        Name = "Troubleshooting"
//    },
//                new TicketType()
//                {
//                    Name = "HelpDesk"
//                });



//    //Seed Ticket Priorities Table
//    context.TicketProrities.AddOrUpdate(t => t.Id,
//    new TicketPriority()
//    {
//        Name = "Urgent"
//    },
//    new TicketPriority()
//    {
//        Name = "Important"
//    },
//     new TicketPriority()
//     {
//         Name = "Standard"
//     },

//     new TicketPriority()
//     {
//         Name = "Low"
//     });


//    //Seed Ticket Status Table
//    context.TicketStatuses.AddOrUpdate(t => t.Id,
//    new TicketStatus()
//    {
//        Name = "Opened"
//    },
//    new TicketStatus()
//    {
//        Name = "Being Worked On"
//    },
//     new TicketStatus()
//     {
//         Name = "On Hold"
//     },

//     new TicketStatus()
//     {
//         Name = "Closed"
//     }

//    );

//}
//    }
//}
