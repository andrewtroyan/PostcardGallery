using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace FinalProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        [Required]
        public long AverageRating
        {
            get
            {
                int postcardsAverageRaiting = Postcards.Count > 0 ?
                    (int)(Postcards.Average(p => p.AverageRating)) : 0;
                int likes = CreatedComments.Count > 0 ? CreatedComments.Sum(c =>
                     c.Likers.Where(l => l.Id != Id).ToList().Count) : 0;
                return postcardsAverageRaiting + likes;
            }

            set { }
        }

        public virtual ICollection<Medal> Medals { get; set; }

        public virtual ICollection<Postcard> Postcards { get; set; }

        public virtual ICollection<Comment> CreatedComments { get; set; }

        public virtual ICollection<Comment> LikedComments { get; set; }

        public virtual ICollection<Rating> GivenRatings { get; set; }

        public ApplicationUser(): base()
        {
            Medals = new List<Medal>();
            Postcards = new List<Postcard>();
            CreatedComments = new List<Comment>();
            LikedComments = new List<Comment>();
            GivenRatings = new List<Rating>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Medal> Medals { get; set; }

        public DbSet<Postcard> Postcards { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<HashTag> HashTags { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Template> Templates { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
           
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}