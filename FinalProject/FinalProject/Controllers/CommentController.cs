using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using FinalProject.Classes_with_extension_methods;
using Microsoft.AspNet.Identity;

namespace FinalProject.Controllers
{
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
    }
}