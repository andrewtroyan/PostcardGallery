using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Medal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }


        [Required]
        public string ImageUrl { get; set; }

        public virtual ICollection<ApplicationUser> RelatedUsers { get; set; }

        public Medal()
        {
            RelatedUsers = new List<ApplicationUser>();
        }
    }
}