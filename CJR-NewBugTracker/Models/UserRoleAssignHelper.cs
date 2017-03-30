using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web;

namespace CJR_NewBugTracker.Models
{
    public class UserRoleAssignHelper
    {
        //UserManger Provides the methods for managering roles
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        private ApplicationDbContext db = new ApplicationDbContext();

        //Checks to see if a user is assign a specific role
        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }

        //Generates a List of User Roles
        public ICollection<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }

        //Adds a User to the Role
        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        //Removes User from a Role
        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        //List All Users in a specific role
        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach(var user in List)
            {
            if (IsUserInRole(user.Id, roleName))
                 {
                resultList.Add(user);

                 }
            }
            return resultList;
        }

        //List all users that are NOT in a specific role

        public ICollection<ApplicationUser> UserNotInRole(string roleName)
        {
        var resultList = new List<ApplicationUser>();
         var List = userManager.Users.ToList();
            foreach(var user in List)
             {
                if (!IsUserInRole(user.Id, roleName))
             resultList.Add(user);
             }
            return resultList;
        }
    }
}