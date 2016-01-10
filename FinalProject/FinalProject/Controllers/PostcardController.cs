using FinalProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Classes_with_extension_methods;
using FinalProject.LuceneSearch;
using FinalProject.Filters;
using System.Collections;
using System.Threading;

namespace FinalProject.Controllers
{
    [Culture]
    public class PostcardController : Controller
    {
        private ApplicationDbContext dataBase = new ApplicationDbContext();

        [Culture]
        [HttpGet]
        public ActionResult GetPostcardPage(int? id)
        {
            if (id == null)
            {
                ViewBag.Reason = Resources.Translations.YouDidNotEnterPostcardId;
                return View("Error");
            }
            var postcard = dataBase.Postcards.SingleOrDefault(p =>
                p.Id == id);
            if (postcard == null)
            {
                ViewBag.Reason = Resources.Translations.ThereAreNoImageWithGivenId;
                return View("Error");
            }

            if (Request.IsAuthenticated)
            {
                var currentUserId = User.Identity.GetUserId();
                var rating = postcard.Ratings.SingleOrDefault(r => r.RaterId ==
                    currentUserId);
                ViewBag.RatingValue = rating == null ? 0 : rating.Value;
            }
            return View(postcard);
        }

        [Authorize]
        public JsonResult RatePostcard(int? postcardId, int? rating)
        {
            if (postcardId == null || rating == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            Postcard postcard = dataBase.Postcards.SingleOrDefault(p =>
                p.Id == postcardId.Value);
            if (postcard == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            postcard.Rate(User.Identity.GetUserId(), rating.Value);
            ProcessMedalsForRatings(postcard);
            dataBase.SaveChanges();
            return Json(new { averageRating = postcard.AverageRating }, 
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPostcardsPage(string userId, int? pageSize, int? postcardPage)
        {
            if (userId == null || pageSize == null || pageSize == 0 || postcardPage == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            int postcardsToSkip = postcardPage.Value * pageSize.Value;
            var result = PostcardSearcher.Search(userId, "OwnerId").Skip(postcardsToSkip).
                Take(pageSize.Value).Select(p => new { databaseId = p.Id, imagePath = 
                p.ImagePath, jsonPath = p.JsonPath, name = p.Name });
            return Json( result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreatePostcard()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return View(db.Templates);
        }

        [HttpGet]
        [Authorize]
        public ActionResult LoadTemplate(int? templateId)
        {
            if (templateId == null)
            {
                return new EmptyResult();
            }
            ApplicationDbContext db = new ApplicationDbContext();
            Template template = db.Templates.Single(t => t.Id == templateId);
            if (template == null)
            {
                return new EmptyResult();
            }
            ViewBag.Categories = db.Categories;
            return PartialView(template);
        }

        private void ProcessMedalsForRatings(Postcard postcard)
        {
            ApplicationUser user = dataBase.Users.Single(u =>
                u.Id == postcard.OwnerId);
            var highRatings = postcard.Ratings.Count > 0 ? postcard.Ratings.
                Where(r => r.Value == 5 && r.RaterId != user.Id).ToList().Count : 0;
            Medal medal = null;
            if (highRatings >= 15)
            {
                medal = dataBase.Medals.Single(m => m.Description ==
                    "ForFifteenHighRatings");
                if (user.Medals.SingleOrDefault(m => m.Id == medal.Id) == null)
                {
                    user.Medals.Add(medal);
                }
            }
            else if (highRatings >= 10)
            {
                medal = dataBase.Medals.Single(m => m.Description ==
                    "ForTenHighRatings");
                if (user.Medals.SingleOrDefault(m => m.Id == medal.Id) == null)
                {
                    user.Medals.Add(medal);
                }
            }
            else if (highRatings >= 5)
            {
                medal = dataBase.Medals.Single(m => m.Description ==
                    "ForFiveHighRatings");
                if (user.Medals.SingleOrDefault(m => m.Id == medal.Id) == null)
                {
                    user.Medals.Add(medal);
                }
            }
            dataBase.SaveChanges();
        }
    }
}