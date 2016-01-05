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

        //[HttpGet]
        //public ActionResult GetImagesAll(string value)
        //{
        //    if (value == null)
        //    {
        //        ViewBag.Reason = "You didn't enter anything.";
        //        return View("Error");
        //    }
        //    ViewBag.SearchingValue = value;
        //    var postcardWithHastTag = dataBase.HashTags.Where(h =>
        //        h.Value.Contains(value.Trim())).Select(h => h.RelatedPostcards).
        //        Cast<Postcard>().Distinct();
        //    var postcardWithComments = dataBase.Comments.Where(c =>
        //        c.Value.Contains(value.Trim())).Select(c => c.RelatedPostcard);
        //    var postcardWithNames = dataBase.Postcards.Where(p =>
        //        p.Name.Contains(value.Trim()));
        //    //if (currentHashTag != null)
        //    //{
        //    //    return View(currentHashTag);
        //    //}
        //    //else
        //    //{
        //    //    ViewBag.Reason = "There are no images with given hash tag.";
        //    //    return View("Error");
        //    //}
        //}
    }
}