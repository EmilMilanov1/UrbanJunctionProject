
using UrbanJunction.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data.Configuration;

namespace UrbanJunction.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

       public DbSet<Post> Posts { get; set; }
       public DbSet<Subcategory> Subcategories { get; set; }
       public DbSet<Topic> Topics { get; set; }
       

        protected override void OnModelCreating(ModelBuilder builder)
        {

			builder.ApplyConfiguration(new TopicConfiguration());         
			builder.ApplyConfiguration(new SubcategoryConfiguration());   
			builder.ApplyConfiguration(new UrbanUserConfiguration());    
			builder.ApplyConfiguration(new PostConfiguration());          


			base.OnModelCreating(builder); // Is needed
										   // Topic
			builder.Entity<Topic>(entity =>
			{
				entity.HasKey(t => t.Id);

				entity.Property(t => t.Name)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(t => t.Description)
					.HasMaxLength(1000);
			});

			// Subcategory
			builder.Entity<Subcategory>(entity =>
			{
				entity.HasKey(s => s.Id);

				entity.Property(s => s.Name)
					.IsRequired()
					.HasMaxLength(100);

				entity.HasOne(s => s.Topic)
					.WithMany(t => t.Subcategories)
					.HasForeignKey(s => s.TopicId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			// Post
			builder.Entity<Post>(entity =>
			{
				entity.HasKey(p => p.Id);

				entity.Property(p => p.Title)
					.IsRequired()
					.HasMaxLength(150);

				entity.Property(p => p.Content)
					.HasMaxLength(4000);

				entity.HasOne(p => p.User)
					.WithMany(u => u.Posts)
					.HasForeignKey(p => p.UserId)
					.OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete of user

				entity.HasOne(p => p.Subcategory)
					.WithMany(s => s.Posts)
					.HasForeignKey(p => p.SubcategoryId)
					.OnDelete(DeleteBehavior.Cascade);
			});
		}
    }
}
