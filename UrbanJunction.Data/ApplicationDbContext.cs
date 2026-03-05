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

            builder.Entity<UserFollow>()
                .HasKey(f => new { f.FollowerId, f.FollowingId });

            builder.Entity<UserFollow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserFollow>()
                .HasOne(f => f.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<PostImage> PostImages { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
    }
}