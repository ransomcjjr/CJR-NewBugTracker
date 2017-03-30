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
    public class ProjectsController : Controller
    {
         ApplicationDbContext db = new ApplicationDbContext();
        private ProjectAssignHelper helper = new ProjectAssignHelper();
        // GET: Projects
        [Authorize]
        public ActionResult Index()
        {
            //return View(db.Projects.ToList());
            {

                if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
                {
                    var projList = db.Projects.ToList().OrderByDescending(p => p.Id);
                    return View(projList);
                }
                else
                {
                    var projList = helper.ListUserProjects(User.Identity.GetUserId());
                    return View(projList);
                }
            }
        }

        // GET: Projects/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = db.Projects.Find(id);            
            if (project == null)
            {
                return HttpNotFound();
            }

            // Get login User
            var loginUser = User.Identity.GetUserId();
 
            // Initialize ProjectAssignHelper class
            ProjectAssignHelper PAH = new ProjectAssignHelper();

            //Check to see if user is assigned to project
            var AssignedProject = PAH.IsUserOnProject(loginUser, project.Id);
            return View(project);
        }


        // GET: Projects/Create
       [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
       [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
       // [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: ProjectAssign
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult ProjectAssign(int ProjectId)
        {
            var project = db.Projects.Find(ProjectId);
            var projectModel = new ProjectAssignViewModel();
            projectModel.project = project;

            projectModel.SelectUsers = project.User.Select(p => p.Id).ToArray();
            projectModel.Users = new MultiSelectList(db.Users, "Id", "FullName", projectModel.SelectUsers);

            return View(projectModel);
        }

        //POST: ProjectAssign
        [HttpPost]
        public ActionResult ProjectAssign(ProjectAssignViewModel Model)
        {
            ProjectAssignHelper helper = new ProjectAssignHelper();
            foreach (var userrmv in db.Users.Select(u => u.Id).ToList())
            {
                //TODO: This keeps blowing up needs fixed project Id coming over null
                helper.RemoveUserFromProject(userrmv, Model.project.Id);

            }

            if (Model.SelectUsers != null)
            {
                foreach (var Useradd in Model.SelectUsers)
                {
                    helper.AddUserToProject(Useradd, Model.project.Id);
                }
            }

            ViewBag.Confirm = "Project assigned users has been successfully modified";

            return RedirectToAction("Details", new { id = Model.project.Id });
        }

        //Garbage Disposel
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
