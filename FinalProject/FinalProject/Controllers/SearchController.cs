using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext dataBase = new ApplicationDbContext();

        [HttpGet]
        public ActionResult GetImagesByHashTag(string hashTag)
        {
            if (hashTag == null)
            {
                ViewBag.Reason = "You didn't enter a hash tag.";
                return View("Error");
            }
            ViewBag.HashTag = hashTag;
            var currentHashTag = dataBase.HashTags.SingleOrDefault
                (h => h.Value == hashTag);
            if (currentHashTag != null)
            {
                return View(currentHashTag);
            }
            else
            {
                ViewBag.Reason = "There are no images with given hash tag.";
                return View("Error");
            }
        }
    }
}