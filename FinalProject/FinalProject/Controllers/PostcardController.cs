﻿using FinalProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Classes_with_extension_methods;
using FinalProject.LuceneSearch;

namespace FinalProject.Controllers
{
    public class PostcardController : Controller
    {
        private ApplicationDbContext dataBase = new ApplicationDbContext();

        [HttpGet]
        public ActionResult GetPostcardPage(int? id)
        {
            if (id == null)
            {
                ViewBag.Reason = "You didn't choose an image.";
                return View("Error");
            }
            var postcard = dataBase.Postcards.SingleOrDefault(p =>
                p.Id == id);
            if (postcard == null)
            {
                ViewBag.Reason = "There are no image with given id.";
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
                Take(pageSize.Value).Select(p => new { databaseId = p.Id, thumbnailUrl = 
                p.ThumbnailUrl, name = p.Name });
            return Json( result, JsonRequestBehavior.AllowGet);
        }
    }
}