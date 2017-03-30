using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJR_NewBugTracker.Models
{
    public class TotalValuesHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public int GetTotalProjects()
        {
            return db.Projects.Count();
        }
        public int GetTotalTickets()
        {
            return db.Tickets.Count();
        }
        
        public int GetTotalComments()
        {
            return db.TicketComments.Count();
        }

        public int GetTotalNotifications()
        {
           return db.TicketNotifications.Count();
        }

        public string GetPercentBugReport()
        {
            var valueTotal = db.Tickets.Count();
            var Bugs = db.Tickets.Count(t => t.TicketTypeId == 1);
            string value = GetPercentage(Bugs, valueTotal, 2);
            return value;

            //Bug report = 1
            //Troubleshooting = 2
            //Help Desk = 3
            //Project Update = 4
        }
        public string GetTroubleShootReport()
        {
            var valueTotal = db.Tickets.Count();
            var tshoot = db.Tickets.Count(t => t.TicketTypeId == 2);
            string value = GetPercentage(tshoot, valueTotal, 2);
            return value;
        }

        public string GetPercentHelpDeskReport()
        {
            var valueTotal = db.Tickets.Count();
            var Hdesk = db.Tickets.Count(t => t.TicketTypeId == 3);
            string value = GetPercentage(Hdesk, valueTotal, 2);
            return value;
        }
        public string GetPercentProjUpdateReport()
        {
            var valueTotal = db.Tickets.Count();
            var Ureport = db.Tickets.Count(t => t.TicketTypeId == 4);
            string value = GetPercentage(Ureport, valueTotal, 2);
            return value;
        }

        public static String GetPercentage(Int32 value, Int32 total, Int32 places)
        {
            Decimal percent = 0;
            String retval = string.Empty;
            String strplaces = new String('0', places);

            if (value == 0 || total == 0)
            {
                percent = 0;
            }

            else
            {
                percent = Decimal.Divide(value, total) * 100;

                if (places > 0)
                {
                    strplaces = "." + strplaces;
                }
            }

            retval = percent.ToString("#" + strplaces);

            return retval;
        }
    }
}