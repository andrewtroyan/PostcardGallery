using FinalProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Classes_with_extension_methods;

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
        public ActionResult RatePostcard(int? postcardId, int? rating)
        {
            if (postcardId == null || rating == null)
            {
                ViewBag.Reason = "Invalid arguments.";
                return View("Error");
            }
            Postcard postcard = dataBase.Postcards.SingleOrDefault(p =>
                p.Id == postcardId.Value);
            if (postcard == null)
            {
                ViewBag.Reason = "There is no card with given id.";
                return View("Error");
            }
            postcard.Rate(User.Identity.GetUserId(), rating.Value);
            dataBase.SaveChanges();
            return new EmptyResult();
        }
    }
}