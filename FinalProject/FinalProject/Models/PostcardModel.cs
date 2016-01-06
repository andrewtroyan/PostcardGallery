using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Postcard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string ThumbnailUrl { get; set; }

        [Required]
        public int AverageRating
        {
            get
            {
                if (Ratings.Any())
                {
                    var ratings = Ratings.Where(r => r.RaterId != OwnerId).ToList();
                    return ratings.Count > 0 ? (int)ratings.Average(r => r.Value) : 0;
                }
                return 0;
            }
            set { }
        }

        [Required]
        public DateTime CreationTime { get; set; }

        public string OwnerId { get; set; }
        public virtual ApplicationUser Owner { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<HashTag> HashTags { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public Postcard()
        {
            CreationTime = DateTime.Now;
            Comments = new List<Comment>();
            HashTags = new List<HashTag>();
            Ratings = new List<Rating>();
        }
    }
}