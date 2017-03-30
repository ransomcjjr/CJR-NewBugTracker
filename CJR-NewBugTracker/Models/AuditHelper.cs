using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CJR_NewBugTracker.Models;
namespace CJR_NewBugTracker.Models
{
    public class AuditHelper
    {
        private ApplicationDbContext dbAudit = new ApplicationDbContext();
        private UserRoleAssignHelper helper = new UserRoleAssignHelper();


        //Compares and Writes Ticket Value Changes to Ticket Audit Log
        public async System.Threading.Tasks.Task<bool> TicketValueChanged(int intTicketId, string strTitle, string strDescription, int intStatus, int intType, int intPriority, string strAssignedTo, string strCurrentUser)
        {
            var objOldValues = dbAudit.Tickets.Find(intTicketId);
            bool hasChanged = false;

            //Title
            if (objOldValues.Title != strTitle)
            {
                //write to audit log
                WriteToAuditLog(objOldValues.Title, strTitle, strCurrentUser, "Title", intTicketId);
                hasChanged = true;
            }

            //Description
            if (objOldValues.Description != strDescription)
            {
                //write to audit log
                WriteToAuditLog(objOldValues.Description, strDescription, strCurrentUser, "Description", intTicketId);
                hasChanged = true;
            }

            //Ticket Status
            await TicketStatusChange(objOldValues.TicketStatusId, intStatus, strCurrentUser, intTicketId);

            //Ticket Type
            if (objOldValues.TicketTypeId != intType)
            {
                //db lookup for type name
                var OldTypeValue = dbAudit.TicketTypes.Find(objOldValues.TicketTypeId);
                var NewTypeValue = dbAudit.TicketTypes.Find(intType);
                //write to audit log
                WriteToAuditLog(OldTypeValue.Name, NewTypeValue.Name, strCurrentUser, "Ticket Type", intTicketId);
                hasChanged = true;
            }

            //Ticket Priority
            if (objOldValues.TicketPriorityId != intPriority)
            {
                //db lookup for priority name
                var OldPriorityValue = dbAudit.TicketProrities.Find(objOldValues.TicketPriorityId);
                var NewPriorityValue = dbAudit.TicketProrities.Find(intPriority);
                //write to audit log
                WriteToAuditLog(OldPriorityValue.Name, NewPriorityValue.Name, strCurrentUser, "Ticket Priority", intTicketId);
                hasChanged = true;
            }

            //Ticket Assigned To
            bool bolAdmin = helper.IsUserInRole(strCurrentUser, "Admin");
            bool bolManager = helper.IsUserInRole(strCurrentUser, "ProjectManager");

            if ((objOldValues.AssignedToUserId != strAssignedTo) && (bolAdmin == true || bolManager == true))
            {
                //db lookup for user names
                var OldUserValue = dbAudit.Users.Find(objOldValues.AssignedToUserId);
                var NewUserValue = dbAudit.Users.Find(strAssignedTo);
                //write to audit log
                WriteToAuditLog(OldUserValue.FullName, NewUserValue.FullName, strCurrentUser, "Assign To User", intTicketId);
                hasChanged = true;

                NotificaitonHelper NFH = new NotificaitonHelper();
                await NFH.NewTicketAssignedNotification(intTicketId,strAssignedTo);

            }


            if (hasChanged == true)
            {
                NotificaitonHelper note = new NotificaitonHelper();
                await note.TicketEditedNotification(intTicketId);
                return true;
            }
            else
            {
                return false;
            }
        }

        //Write Status Changes to Ticket Audit Log
       public async System.Threading.Tasks.Task<bool> TicketStatusChange(int intOldStatusId, int intNewStatusId, string strUser, int intTicket)
        {
            if (intOldStatusId != intNewStatusId)
            {
                //db lookup for status name
                var OldStatValue = dbAudit.TicketStatuses.Find(intOldStatusId);
                var NewStatValue = dbAudit.TicketStatuses.Find(intNewStatusId);
                //write to audit log
                WriteToAuditLog(OldStatValue.Name, NewStatValue.Name, strUser,"Status",intTicket);

                //send email fo status change
                NotificaitonHelper note = new NotificaitonHelper();
               await note.StatusChangeNotification(intTicket, NewStatValue.Name);
            }
            return true;
        }

        //Writes Audit Values to the History Table
        public bool WriteToAuditLog(string strOldValue, string strNewValue, string strUser,string strProperty, int intTicketNum)
        {
            //TODO: Correct Syntax to Add record to history table

            TicketHistory ticketAudit = new TicketHistory();
            ticketAudit.TicketId = intTicketNum;
            ticketAudit.Property = strProperty;
            ticketAudit.OldValue = strOldValue;
            ticketAudit.NewValue = strNewValue;
            ticketAudit.Changed = DateTime.Now;
            ticketAudit.UserId = strUser;
            dbAudit.TicketHistories.Add(ticketAudit);
            dbAudit.SaveChanges();
            return true;
        }
    }
}