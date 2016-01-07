using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Comparers;
using FinalProject.LuceneSearch;
using FinalProject.Filters;

namespace FinalProject.Controllers
{
    [Culture]
    public class SearchController : Controller
    {
        private ApplicationDbContext dataBase = new ApplicationDbContext();

        [HttpGet]
        public ActionResult GetImagesByHashTag(string hashTag)
        {
            if (hashTag == null)
            {
                ViewBag.Reason = String.Format("{0}.", Resources.Translations.YouDidNotEnterHashTag);
                return View("Error");
            }
            var currentHashTag = dataBase.HashTags.SingleOrDefault
                (h => h.Value == hashTag);
            if (currentHashTag == null)
            {
                ViewBag.Reason = Resources.Translations.ThereIsNoSuchHashTags;
                return View("Error");
            }
            ViewBag.HashTag = hashTag;
            return View(currentHashTag);
        }

        [HttpGet]
        public ActionResult GetImagesAll(string value)
        {
            if (value == null)
            {
                ViewBag.Reason = String.Format("{ 0}.", Resources.Translations.YouDidNotEnterAnything);;
                return View("Error");
            }
            string searchingValue = value.Trim();
            int hitsLimit = 100;
            var postcardWithNames = PostcardSearcher.Search(value, "Name", hitsLimit).
                ToList();
            if (postcardWithNames.Count >= hitsLimit)
            {
                return View(postcardWithNames);
            }
            PostcardComparer comparer = new PostcardComparer();
            var postcardWithHastTag = dataBase.HashTags.Where(h =>
                h.Value.Contains(searchingValue)).Select(h => h.RelatedPostcards).
                SelectMany(c => c).AsEnumerable();
            var postcardWithComments = dataBase.Comments.Where(c =>
                c.Value.Contains(searchingValue)).Select(c => c.RelatedPostcard).
                AsEnumerable();
            var uniquePostcards = postcardWithHastTag.Union(postcardWithComments,
                comparer).Union(postcardWithNames, comparer);
            ViewBag.SearchingValue = value;
            return View(uniquePostcards);
        }
    }
}