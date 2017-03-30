using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CJR_NewBugTracker.Models
{
    public class ProjectAssignHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Checks to see if user has been assigned to the project
        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var user = project.User.Any(u => u.Id == userId);
            return (user);

        }

        //Adds User to a project
        public void AddUserToProject(string userId, int projectId)
        {
            ApplicationUser user = db.Users.Find(userId);
            Project project = db.Projects.Find(projectId);
            project.User.Add(user);
            db.SaveChanges();
        }

        //Removes User From Promect

        public void RemoveUserFromProject(string userId, int projectId)
        {
            ApplicationUser user = db.Users.Find(userId);
            Project project = db.Projects.Find(projectId);
            project.User.Remove(user);
            db.SaveChanges();
        }

        //Shows a list of projects a user is assigned to
        public List<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            return user.Projects.ToList();
        }

        //Shows a list of all users on a project
        public List<ApplicationUser> ListUsersOnProject(int projectId)
        {
            Project project = db.Projects.Find(projectId);
            return project.User.ToList();
        }

        //Shows a list of all users in a specific role on a specific project
        public List<ApplicationUser> ListDevelopersOnProject(int projectId, string roleName)
        {
            Project project = db.Projects.Find(projectId);
            var projectUserList = project.User;

            var projectDev = new List<ApplicationUser>();
            var roleHelper = new UserRoleAssignHelper();

            foreach (var user in projectUserList)
            {
                if (roleHelper.IsUserInRole(user.Id,roleName))
                {
                    projectDev.Add(user);
                }
            }

            return projectDev;
        }


        //Returns a List Of User NOT in a Project
        public List<ApplicationUser> ListUsersNotOnProject(int projectId)
        {
            // Project project = db.Projects.Find(projectId);
            //var userObj = project.User;
            // return db.Users.Where(u => !userObj.Contains(u)).ToList();

            //Alternatice Lamda Code for the above 3 lines
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();

        }
    }
}