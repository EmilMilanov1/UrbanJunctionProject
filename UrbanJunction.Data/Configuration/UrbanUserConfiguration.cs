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
	public class UrbanUserConfiguration : IEntityTypeConfiguration<UrbanUser>
	{
		public void Configure(EntityTypeBuilder<UrbanUser> builder)
		{
			builder.HasData(UrbanUserSeeder.SeedUsers());
		}
	}
}
