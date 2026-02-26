using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrbanJunction.Data.Models;

namespace UrbanJunction.Data.Configuration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.Content)
                .HasMaxLength(4000);

            builder.HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete of user

            builder.HasOne(p => p.Subcategory)
                .WithMany(s => s.Posts)
                .HasForeignKey(p => p.SubcategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
