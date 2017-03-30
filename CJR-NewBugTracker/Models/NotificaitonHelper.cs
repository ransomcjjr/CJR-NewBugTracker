using CJR_NewBugTracker;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CJR_NewBugTracker.Models
{
    public class NotificaitonHelper
    {
        private EmailService email = new EmailService();
        private ApplicationDbContext dbEmail = new ApplicationDbContext();

        public async Task NewTicketAssignedNotification(int intTicketId,string strNewAssigned)//Emails user when ticket assignment has changed
        {
            var Tick = dbEmail.Tickets.Find(intTicketId);
            var OldDevUser = dbEmail.Users.Find(Tick.AssignedToUserId);
            var NewDevUser = dbEmail.Users.Find(strNewAssigned);

            //emails old user of assigement change
            if (OldDevUser.FirstName != "Unassigned")
            {
                var msg = new IdentityMessage();

                msg.Destination = OldDevUser.Email;
                msg.Subject = ("Bug Tracker UPDATE: Assignment Change.");
                msg.Body = ("You have been unassigned to ticket " + intTicketId + " " + Tick.Title + " " + Tick.Description);

                await email.SendAsync(msg);

                AddToNotificationTable(intTicketId, OldDevUser.Id);

            }

            //emails new user of new assignment
            if (NewDevUser.FirstName != "Unassigned")
            {
                var msg = new IdentityMessage();

                msg.Destination = NewDevUser.Email;
                msg.Subject = ("Bug Tracker UPDATE: New Assignment.");
                msg.Body = ("You have been assigned to ticket " + intTicketId + " " + Tick.Title + " " + Tick.Description);

                await email.SendAsync(msg);
                AddToNotificationTable(intTicketId, NewDevUser.Id);
            }
        }
        public async Task NewAttachementNotification(int intTicketId, string attachemnt) //send when a new attachement is added to a ticket
        {
            var Tick = dbEmail.Tickets.Find(intTicketId);
            var DevUser = dbEmail.Users.Find(Tick.AssignedToUserId);

            if (DevUser.FirstName != "Unassigned")
            {
                var msg = new IdentityMessage();

                msg.Destination = DevUser.Email;
                msg.Subject = ("Bug Tracker UPDATE: New Attachment " + attachemnt);
                msg.Body = ("A New attachment has been added to ticket " + intTicketId + "-" + Tick.Title + "-" + Tick.Description);

                await email.SendAsync(msg);
                AddToNotificationTable(intTicketId, DevUser.Id);
            }
        }
        public async Task NewCommentNotification(int intTicketId, string Comment)//Email when a new comment has been added
        {
            var Tick = dbEmail.Tickets.Find(intTicketId);
            var DevUser = dbEmail.Users.Find(Tick.AssignedToUserId);

            if (DevUser.FirstName != "Unassigned")
            {
                var msg = new IdentityMessage();

                msg.Destination = DevUser.Email;
                msg.Subject = ("Bug Tracker UPDATE: New Comment.");
                msg.Body = ("A new comment has been added to ticket " + intTicketId + " " + Tick.Title + " " + Tick.Description + "<br>" + Comment);

                await email.SendAsync(msg);
                AddToNotificationTable(intTicketId, DevUser.Id);
            }
        }
        public async Task StatusChangeNotification(int intTicketId,string strStatus)//Email when status has changed
        {
            var Tick = dbEmail.Tickets.Find(intTicketId);
            var DevUser = dbEmail.Users.Find(Tick.AssignedToUserId);

            if (DevUser.FirstName != "Unassigned")
            {
                var msg = new IdentityMessage();

                msg.Destination = DevUser.Email;
                msg.Subject = ("Bug Tracker UPDATE: Ticket Status Change");
                msg.Body = ("The status of ticket " + intTicketId + " " + Tick.Title + " " + Tick.Description + "<br> has change to " + strStatus);

                await email.SendAsync(msg);

                AddToNotificationTable(intTicketId, DevUser.Id);
            }
        }
        public async Task TicketEditedNotification(int intTicketId)// Send email when changes have been made to the ticket
        {
            var Tick = dbEmail.Tickets.Find(intTicketId);
            var DevUser = dbEmail.Users.Find(Tick.AssignedToUserId);

            if (DevUser.FirstName != "Unassigned")
            {
                var msg = new IdentityMessage();

                msg.Destination = DevUser.Email;
                msg.Subject = ("Bug Tracker UPDATE: Ticket Has Been Edited");
                msg.Body = ("The details of ticket " + intTicketId + " " + Tick.Title + " " + Tick.Description + " have been edited");

                await email.SendAsync(msg);
                AddToNotificationTable(intTicketId, DevUser.Id);
            }
        }
        public bool AddToNotificationTable(int intTicketId,string strUserId) //Writes notifications to the Ticket Notifications table.
        {
            TicketNotification tickNote = new TicketNotification();
            tickNote.TicketId = intTicketId;
            tickNote.UserId = strUserId;
            dbEmail.TicketNotifications.Add(tickNote);
            dbEmail.SaveChanges();
            return true;
        }

    }
}