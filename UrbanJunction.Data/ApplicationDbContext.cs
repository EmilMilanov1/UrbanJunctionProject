
using UrbanJunction.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data.Configuration;

namespace UrbanJunction.Data
{
    public class ApplicationDbContext : IdentityDbContext<UrbanUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TopicConfiguration());
            builder.ApplyConfiguration(new SubcategoryConfiguration());
            builder.ApplyConfiguration(new PostConfiguration());

            base.OnModelCreating(builder); // Is needed
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<PostImage> PostImages { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
    }
}
