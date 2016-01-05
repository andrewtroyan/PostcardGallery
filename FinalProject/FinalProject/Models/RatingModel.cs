using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Value { get; set; }

        public string RaterId { get; set; }
        public virtual ApplicationUser Rater { get; set; }

        public int? RelatedPostcardId { get; set; }
        public virtual Postcard RelatedPostcard { get; set; }
    }
}