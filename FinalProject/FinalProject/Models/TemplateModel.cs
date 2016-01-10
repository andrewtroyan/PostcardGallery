using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Template
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string JsonPath { get; set; }

        public virtual ICollection<Postcard> RelatedPostcards { get; set; }

        public Template()
        {
            RelatedPostcards = new List<Postcard>();
        }
    }
}