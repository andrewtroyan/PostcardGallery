using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Filters;

namespace FinalProject.Controllers
{
    [Culture]
    public class UserController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult GetUserPage(string userId)
        {
            if (userId == null)
            {
                ViewBag.Reason = "You didn't enter user's id.";
                return View("Error");
            }
            ApplicationUser user = db.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                ViewBag.Reason = "User doesn't exist.";
                return View("Error");
            }
            return View(user);
        }
    }
}