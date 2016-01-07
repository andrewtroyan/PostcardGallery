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
        public ActionResult GetUserPage(string userId)
        {
            if (userId == null)
            {
                ViewBag.Reason = String.Format("{0}.", Resources.Translations.YouDidNotEnterUserId);
                return View("Error");
            }
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = db.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                ViewBag.Reason = String.Format("{0}.", Resources.Translations.UserDoesNotExist);
                return View("Error");
            }
            return View(user);
        }
    }
}