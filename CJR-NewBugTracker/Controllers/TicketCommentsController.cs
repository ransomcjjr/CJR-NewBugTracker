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

namespace CJR_NewBugTracker.Controllers
{
    [RequireHttps]
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketComments
        [Authorize]
        public ActionResult Index()
        {
            var ticketComments = db.TicketComments.Include(t => t.Ticket);
            return View(ticketComments.ToList());
        }

        // GET: TicketComments/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            return View(ticketComment);
        }

        // GET: TicketComments/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            ViewBag.ticket = ticket;
            return View();
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Create([Bind(Include = "Comment,TicketId")] TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {
                var curUser = User.Identity.GetUserId();
                ticketComment.Created = DateTime.Now;
                ticketComment.UserId = curUser;
                db.TicketComments.Add(ticketComment);
                db.SaveChanges();

                //sent email notification
                NotificaitonHelper note = new NotificaitonHelper();
                await note.NewCommentNotification(ticketComment.TicketId, ticketComment.Comment);

                return RedirectToAction("Details", "Tickets", new { id = ticketComment.TicketId });
            }

            Ticket ticket = db.Tickets.Find(ticketComment.TicketId);
            ViewBag.ticket = ticket;
            return View(ticketComment);
        }

        // GET: TicketComments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            var currentTicket = db.Tickets.Find(ticketComment.TicketId);
            ViewBag.ticket = currentTicket;
            return View(ticketComment);
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Comment,Created,TicketId,UserId")] TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticketComment.TicketId });
            }
            Ticket ticket = db.Tickets.Find(ticketComment.TicketId);
            ViewBag.ticket = ticket;
            return View(ticketComment);
        }

        // GET: TicketComments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            var currentTicket = db.Tickets.Find(ticketComment.TicketId);
            ViewBag.ticket = currentTicket;
            return View(ticketComment);
        }

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketComment ticketComment = db.TicketComments.Find(id);
            db.TicketComments.Remove(ticketComment);
            db.SaveChanges();
            return RedirectToAction("Details", "Tickets", new { id = ticketComment.TicketId });
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
