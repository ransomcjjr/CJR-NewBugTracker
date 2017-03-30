using CJR_NewBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CJR_BugTracker.Controllers
{
    [RequireHttps]
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        UserRoleAssignHelper helper = new UserRoleAssignHelper();

        // GET: Admin Index
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        { 
            List<AdminUserViewModel> userList = new List<AdminUserViewModel>();
             foreach (var users in db.Users.ToList())
            {
            var userCollection = new AdminUserViewModel();
            userCollection.User = users;
            userCollection.Role = helper.ListUserRoles(users.Id).ToList();
            userList.Add(userCollection);
            }
            return View(userList);
        }


        //GET: Admin/SelectRole/5
       [Authorize(Roles = "Admin")]
        public ActionResult SelectRole(string id)
    {
        var user = db.Users.Find(id);
        var roleUser = new UserRoleViewModel();
        roleUser.Id = user.Id;
        roleUser.FirstName = user.FirstName;
        roleUser.LastName = user.LastName;
        roleUser.SelectedRoles = helper.ListUserRoles(user.Id).ToArray();
        roleUser.UserRoles = new MultiSelectList(db.Roles, "Name", "Name", roleUser.SelectedRoles);

        return View(roleUser);
    }


        //POST: Admin/SelectRole
        [HttpPost]
        public ActionResult SelectRole(UserRoleViewModel model)
        {
            var user = db.Users.Find(model.Id);
            foreach (var rolermv in db.Roles.Select(r => r.Name).ToList())
            {
                helper.RemoveUserFromRole(user.Id, rolermv);
            }

            if (model.SelectedRoles != null)
            { 
                foreach (var roleadd in model.SelectedRoles)
                {
                    helper.AddUserToRole(user.Id, roleadd);
                }
             }

            ViewBag.Confirm = "User's role has been successfully modified";
            return RedirectToAction("Index");
        }


       // public ActionResult mypage(string Name)
       // {
       //    Lamda Example
         //   var example = db.Projects.Where(p => p.Name == Name);
         //   THIS IS THE SAME AS THE LAMDA ABOVE
         //   var c = String.Empty;
         //   foreach(var p in db.Projects)
          //  {
           //     if(p.Name == Name)
           //     {
            //        c = p.Name;
            //    }
           // }
       // }
    }
}