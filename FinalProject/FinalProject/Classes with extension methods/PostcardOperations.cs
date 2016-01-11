using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinalProject.Models;
using Microsoft.AspNet.Identity;
using FinalProject.LuceneSearch;

namespace FinalProject.Classes_with_extension_methods
{
    static public class PostcardOperations
    {
        public static void AddHashTags(this Postcard postcard, string hashTags)
        {
            foreach (var tag in hashTags.Split(',').Distinct())
            {
                postcard.AddHashTag(tag.Trim());
            }
        }

        public static void AddHashTag(this Postcard postcard, string hashTag)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                postcard = db.Postcards.Single(p => p.Id == postcard.Id);
                HashTag existingHashTag = db.HashTags.SingleOrDefault(h =>
                    h.Value == hashTag);
                if (existingHashTag == null)
                {
                    existingHashTag = new HashTag
                    {
                        Value = hashTag
                    };
                    HashTagSearcher.AddUpdateLuceneIndex(existingHashTag);
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