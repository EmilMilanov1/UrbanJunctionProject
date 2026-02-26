using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanJunction.Data.Models;
using UrbanJunction.Data.Seeding;

namespace UrbanJunction.Data.Configuration
{
	public class SubcategoryConfiguration : IEntityTypeConfiguration<Subcategory>
	{
		public void Configure(EntityTypeBuilder<Subcategory> builder)
		{
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(s => s.Topic)
                .WithMany(t => t.Subcategories)
                .HasForeignKey(s => s.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(SubcategorySeeder.SeedSubcategories());
		}
	}
}
