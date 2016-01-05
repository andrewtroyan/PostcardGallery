using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;

namespace FinalProject.Classes_with_extension_methods
{
    public static class CommentOperations
    {
        public static void Like(this Comment comment, string userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                comment = db.Comments.Single(c => c.Id == comment.Id);
                ApplicationUser user = db.Users.Single(u => u.Id == userId);
                comment.Likers.Add(user);
                db.SaveChanges();
            }
        }

        public static void Dislike(this Comment comment, string userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                comment = db.Comments.Single(c => c.Id == comment.Id);
                ApplicationUser user = db.Users.Single(u => u.Id == userId);
                comment.Likers.Remove(user);
                db.SaveChanges();
            }
        }
    }
}