using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;
using Microsoft.AspNet.Identity;

namespace FinalProject.Classes_with_extension_methods
{
    static public class PostcardOperations
    {
        public static void AddHashTag(this Postcard postcard, string hashTag)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                HashTag existingHashTag = db.HashTags.SingleOrDefault(h =>
                    h.Value == hashTag);
                if (existingHashTag == null)
                {
                    existingHashTag = new HashTag
                    {
                        Value = hashTag
                    };
                    db.HashTags.Add(existingHashTag);
                }
                postcard.HashTags.Add(existingHashTag);
                db.SaveChanges();
            }
        }

        public static void Rate(this Postcard postcard, string userId, int value)
        {
            Rating rating = postcard.Ratings.SingleOrDefault(r => r.RaterId ==
                userId);
            if (rating == null)
            {
                rating = new Rating
                {
                    RelatedPostcardId = postcard.Id,
                    RaterId = userId,
                    Value = value
                };
                postcard.Ratings.Add(rating);
            }
            else
            {
                rating.Value = value;
            }
        }
    }
}