using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using FinalProject.Classes_with_extension_methods;
using Microsoft.AspNet.Identity;
using FinalProject.Filters;

namespace FinalProject.Controllers
{
    [Culture]
    [Authorize]
    public class CommentController : Controller
    {
        public ActionResult Like(int? commentId)
        {
            if (commentId == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Comment comment = db.Comments.SingleOrDefault(c => c.Id ==
                    commentId);
                if (commentId == null)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
                comment.Like(User.Identity.GetUserId());
                ProcessMedalsForLikes(comment);
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dislike(int? commentId)
        {
            if (commentId == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Comment comment = db.Comments.SingleOrDefault(c => c.Id ==
                    commentId);
                if (commentId == null)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
                comment.Dislike(User.Identity.GetUserId());
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult PostComment(int? postcardId, string value,
            string creationTime)
        {
            if (postcardId == null || value == null || creationTime == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            Comment newComment = new Comment
            {
                OwnerId = User.Identity.GetUserId(),
                RelatedPostcardId = postcardId,
                Value = value,
                CreationTime = creationTime
            };
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Comments.Add(newComment);
                db.SaveChanges();
            }
            return Json(new { id = newComment.Id }, JsonRequestBehavior.AllowGet);
        }

        private void ProcessMedalsForLikes(Comment comment)
        {
            using (ApplicationDbContext dataBase = new ApplicationDbContext())
            {
                ApplicationUser owner = dataBase.Users.Single(u => u.Id ==
                    comment.OwnerId);
                var likes = comment.Likers.Count > 0 ? comment.Likers.
                Where(l => l.Id != owner.Id).ToList().Count : 0;
                Medal medal = null;

                if (likes >= 15)
                {
                    medal = dataBase.Medals.Single(m => m.Description ==
                        "ForFifteenLikes");
                    if (owner.Medals.SingleOrDefault(m => m.Id == medal.Id) == null)
                    {
                        owner.Medals.Add(medal);
                    }
                }
                else if (likes >= 10)
                {
                    medal = dataBase.Medals.Single(m => m.Description ==
                        "ForTenLikes");
                    if (owner.Medals.SingleOrDefault(m => m.Id == medal.Id) == null)
                    {
                        owner.Medals.Add(medal);
                    }
                }
                else if (likes >= 5)
                {
                    medal = dataBase.Medals.Single(m => m.Description ==
                        "ForFiveLikes");
                    if (owner.Medals.SingleOrDefault(m => m.Id == medal.Id) == null)
                    {
                        owner.Medals.Add(medal);
                    }
                }
                dataBase.SaveChanges();
            }
        }
    }
}