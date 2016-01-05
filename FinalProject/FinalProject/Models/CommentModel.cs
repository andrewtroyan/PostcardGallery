using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string CreationTime { get; set; }
        
        public string OwnerId { get; set; }
        [InverseProperty("CreatedComments")]
        public virtual ApplicationUser Owner { get; set; }

        public int? RelatedPostcardId { get; set; }
        public virtual Postcard RelatedPostcard { get; set; }

        [InverseProperty("LikedComments")]
        public virtual ICollection<ApplicationUser> Likers { get; set; }

        public Comment()
        {
            Likers = new List<ApplicationUser>();
            CreationTime = DateTime.UtcNow.ToString("s") + "Z";
        }
    }
}