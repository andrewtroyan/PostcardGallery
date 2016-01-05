using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Comparers;

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
            return View(currentHashTag);
        }

        [HttpGet]
        public ActionResult GetImagesAll(string value)
        {
            if (value == null)
            {
                ViewBag.Reason = "You didn't enter anything.";
                return View("Error");
            }
            string searchingValue = value.Trim();
            PostcardComparer comparer = new PostcardComparer();
            var postcardWithHastTag = dataBase.HashTags.Where(h =>
                h.Value.Contains(searchingValue)).Select(h => h.RelatedPostcards).
                SelectMany(c => c).AsEnumerable();
            var postcardWithComments = dataBase.Comments.Where(c =>
                c.Value.Contains(searchingValue)).Select(c => c.RelatedPostcard).
                AsEnumerable(); ;
            var postcardWithNames = dataBase.Postcards.Where(p =>
                p.Name.Contains(searchingValue)).AsEnumerable(); ;
            var uniquePostcards = postcardWithHastTag.Union(postcardWithComments,
                comparer).Union(postcardWithNames, comparer);
            ViewBag.SearchingValue = value;
            return View(uniquePostcards);
        }
    }
}