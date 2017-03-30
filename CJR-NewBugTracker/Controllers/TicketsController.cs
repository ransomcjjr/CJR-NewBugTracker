using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CJR_NewBugTracker.Models;
using Microsoft.AspNet.Identity;
using System.IO;

namespace CJR_NewBugTracker.Controllers
{
    [RequireHttps]
    public class TicketsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            var loginUser = User.Identity.GetUserId();
            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketStatus).Include(t => t.TicketType).OrderByDescending(t => t.Created);
            ProjectAssignHelper PAH = new ProjectAssignHelper();
            UserRoleAssignHelper URAH = new UserRoleAssignHelper();

           // Determings what project tickets to return to the project details page
            if (User.IsInRole("Admin")) //returns all tickets in project when admin is logged in
            {
                //return View(tickets.OrderByDescending(t => t.Created));
                return View(tickets);
            }
            else if (User.IsInRole("ProjectManager")) //returns all tickets in project when admin is logged in
            {
                var devProjectTickets = tickets.AsQueryable();
                devProjectTickets = devProjectTickets.Where(d => d.Project.User.Any(u => u.Id == loginUser));
                return View(devProjectTickets.OrderByDescending(t => t.Created));
            }
            else if (User.IsInRole("Developer") && User.IsInRole("Submitter")) //Pulls ticket for assigned and ower of ticket in these roles
            {
                ////need to get tickets from owner and assigned the merge distinct
                var devProjectTickets = tickets.AsQueryable();
                devProjectTickets = devProjectTickets.Where(d => d.AssignedToUserId.Contains(loginUser) || d.OwnerUserId.Contains(loginUser));
                return View(devProjectTickets.OrderByDescending(t => t.Created));
            }
            else if (User.IsInRole("Developer")) // pulls tickets that are assigned to the login developer
            {
                //narrow down to only assigned tickets
                var devProjectTickets = tickets.AsQueryable();
                devProjectTickets = devProjectTickets.Where(d => d.AssignedToUserId.Contains(loginUser));
                return View(devProjectTickets.OrderByDescending(t => t.Created));

            }
            else if (User.IsInRole("Submitter")) // pulls tickets where ticket owner is the current logged in user and is in the submitter role.
            {
                var devProjectTickets = tickets.AsQueryable();
                devProjectTickets = devProjectTickets.Where(d => d.OwnerUserId.Contains(loginUser));
                return View(devProjectTickets.OrderByDescending(t => t.Created));
            }
            else
            {
                var devProjectTickets = tickets.AsQueryable();
                devProjectTickets = devProjectTickets.Where(d => d.OwnerUserId.Contains(loginUser));
                return View(devProjectTickets.OrderByDescending(t => t.Created));
            }
        }

        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(int? Id)
        {
            Project project = db.Projects.Find(Id);
            ViewBag.Project = project;
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketProrities, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,ProjectId,TicketTypeId,TicketPriorityId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.TicketStatusId = 1;
                ticket.Created = DateTime.Now;
                ticket.Updated = DateTime.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                var userUnassigned = db.Users.FirstOrDefault(u => u.FirstName == "Unassigned");
                ticket.AssignedToUserId = userUnassigned.Id;
                db.Tickets.Add(ticket);
                db.SaveChanges();


                return RedirectToAction("Details", "Projects", new { id = ticket.ProjectId });
            }

            return View();
        }

        // GET: Tickets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);

            if (ticket == null)
            {
                return HttpNotFound();
            }
            var userUnassigned = db.Users.FirstOrDefault(u => u.FirstName == "Unassigned");
            var pId = ticket.ProjectId;
            var projHelper = new ProjectAssignHelper();
            var projectDevList = projHelper.ListDevelopersOnProject(pId, "Developer");
            projectDevList.Add(userUnassigned);

            ViewBag.AssignedToUserId = new SelectList(projectDevList, "Id", "FullName", ticket.AssignedToUserId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketProrities, "Id", "Name",ticket.TicketPriorityId);

            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,ProjectId,TicketTypeId,TicketPriorityId,AssignedToUserId,TicketStatusId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var UserName = db.Users.Find(ticket.AssignedToUserId);
                var strLoginUser = User.Identity.GetUserId();

                if (ticket.TicketStatusId != 3) //Closed
                {
                    if (UserName.FirstName == "Unassigned")
                    {
                        ticket.TicketStatusId = 1; //status unassigned
                    }
                    else
                    {
                        ticket.TicketStatusId = 2; //assigned ticket
                    }
                    var book1 = db.Tickets.AsNoTracking().Where(m => m.Id == ticket.Id);

        
                }

                //Calls Audit Helper class to write changes to the history table
                AuditHelper audit = new AuditHelper();
                await audit.TicketValueChanged(ticket.Id, ticket.Title, ticket.Description, ticket.TicketStatusId, ticket.TicketTypeId, ticket.TicketPriorityId, ticket.AssignedToUserId, strLoginUser);

                //Set ticket properties and updates ticket
                ticket.Updated = DateTime.Now;
                db.Tickets.Attach(ticket);
                db.Entry(ticket).Property("Title").IsModified = true;
                db.Entry(ticket).Property("Description").IsModified = true;
                db.Entry(ticket).Property("TicketTypeId").IsModified = true;
                db.Entry(ticket).Property("TicketPriorityId").IsModified = true;
                db.Entry(ticket).Property("TicketStatusId").IsModified = true;
                db.Entry(ticket).Property("AssignedToUserId").IsModified = true;
                db.Entry(ticket).Property("Updated").IsModified = true;

                db.SaveChanges();
                return RedirectToAction("Details", "Projects", new { id = ticket.ProjectId });
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        // It Does Not Delete record it just markes it as closed
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        // It Does Not Delete record it just markes it as closed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Delete(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            var strLoginUser = User.Identity.GetUserId();

            //Sends Ticket staus change to history audit log
            AuditHelper audit = new AuditHelper();
            await audit.TicketStatusChange(ticket.TicketStatusId, 3, strLoginUser, id);

            //Updates Status to Closed in the ticket table
            ticket.TicketStatusId = 3;

            db.Tickets.Attach(ticket);
            db.Entry(ticket).Property("TicketStatusId").IsModified = true;
            db.SaveChanges();

            return RedirectToAction("Details", "Projects", new { id = ticket.ProjectId });
        }

        // POST: Ticket/Attachment Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> TicketAttachment([Bind(Include = "TicketId,Description,FilePath")] TicketAttachment Attachment, HttpPostedFileBase pdfFile)
        {
            if (ModelState.IsValid)
            {
                //image validation
                if (PdfUploadValidator.IsWebFriendlyPdf(pdfFile))
                {
                    var filename = Path.GetFileName(pdfFile.FileName);
                    pdfFile.SaveAs(Path.Combine(Server.MapPath("~/Docs/TicketAttachments/"), filename));
                    Attachment.FilePath = "~/Docs/TicketAttachments/" + filename;

                    Attachment.Created = DateTime.Now;
                    Attachment.UserId = User.Identity.GetUserId();
                    db.TicketAttachments.Add(Attachment);
                    db.SaveChanges();

                    // send email notification
                    NotificaitonHelper note = new NotificaitonHelper();
                    await note.NewAttachementNotification(Attachment.TicketId, Attachment.Description);
                    return RedirectToAction("Details", "Tickets", new { id = Attachment.TicketId });
                }


                return RedirectToAction("AttachementError", "Tickets", new { id = Attachment.TicketId });
            }

            return View();
        }

        // GET: Ticket Attachement/Delete/5
        [Authorize]
        public ActionResult DeleteAttachment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketsAttachment = db.TicketAttachments.Find(id);
            if (ticketsAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketsAttachment);
        }

        // POST: Ticket/Delete/5
        // [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile(int id)
        {
           TicketAttachment ticketsAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketsAttachment);
            db.SaveChanges();
            // return RedirectToAction("Index");
            return RedirectToAction("Details", "Tickets", new { id = ticketsAttachment.TicketId });
        }

        //GET: Ticket/AttachementError
        [Authorize]
        public ActionResult AttachementError(int id)
        {
            Ticket tickets = db.Tickets.Find(id);
            return View(tickets);
        }

        //POST: Ticket/AttachementError
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AttachementError(Ticket tickets)
        {
            
            return RedirectToAction("Details", "Tickets", new { id = tickets.Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
