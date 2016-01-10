using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using Microsoft.AspNet.Identity;
using FinalProject.Filters;

namespace FinalProject.Controllers
{
    [Culture]
    public class DataController : Controller
    {
        private ApplicationDbContext dataBase = new ApplicationDbContext();

        [HttpGet]
        public JsonResult GetHashTags()
        {
            var hashTags = dataBase.HashTags.Select(h => new
            {
                text = h.Value,
                weight = h.RelatedPostcards.Count,
                link = "/Search/GetImagesByHashTag?hashTag=" + h.Value
            });
            return Json(hashTags, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLatelyAddedPostcards(int? amount)
        {
            if (amount == null || amount == 0)
            {
                return null;
            }
            else
            {
                var postcards = dataBase.Postcards.OrderByDescending
                    (p => p.CreationTime).Take(amount.Value).Select
                    (p => new {
                        name = p.Name,
                        imagePath = p.ImagePath,
                        jsonPath = p.JsonPath,
                        databaseId = p.Id
                    });
                return Json(postcards, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetTopPostcards(int? amount)
        {
            if (amount == null || amount == 0)
            {
                return null;
            }
            else
            {
                var postcards = dataBase.Postcards.OrderByDescending
                    (p => p.AverageRating).Take(amount.Value).Select
                    (p => new {
                        name = p.Name,
                        imagePath = p.ImagePath,
                        jsonPath = p.JsonPath,
                        databaseId = p.Id
                    });
                return Json(postcards, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetComments(int? postcardId)
        {
            if (postcardId == null)
            {
                return null;
            }
            Postcard postcard = dataBase.Postcards.SingleOrDefault(p => p.Id ==
                postcardId.Value);
            if (postcard == null)
            {
                return null;
            }
            string currentUserId = User.Identity.GetUserId();
            var comments = postcard.Comments.Select(c => new {
                id = c.Id,
                created = c.CreationTime,
                content = c.Value,
                fullname = c.Owner.UserName,
                upvote_count = c.Likers.Count,
                profile_picture_url = "/Content/img/user.png",
                user_has_upvoted = c.Likers.SingleOrDefault(l => l.Id ==
                    currentUserId) != null
            });
            return Json(comments, JsonRequestBehavior.AllowGet);
        }
    }
}