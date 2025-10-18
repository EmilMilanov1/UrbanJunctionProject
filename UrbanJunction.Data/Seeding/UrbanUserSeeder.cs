using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrbanJunction.Data.Models;

namespace UrbanJunction.Data.Seeding
{
	public class UrbanUserSeeder
	{
		public static IEnumerable<UrbanUser> SeedUsers()
		{
			var hasher = new PasswordHasher<UrbanUser>();
            IEnumerable<UrbanUser> users = new List<UrbanUser>()
			{
				new UrbanUser
				{
					Id = "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
					UserName = "Emo",
					NormalizedUserName = "ARTLOVER@URBAN.COM",
					Email = "artlover@urban.com",
					NormalizedEmail = "ARTLOVER@URBAN.COM",
					EmailConfirmed = true,
					PreferredTheme = "Dark",
					PasswordHash = hasher.HashPassword(null, "ArtLover123!")
				},
				new UrbanUser
				{
					Id = "3ad674e3-3797-41ba-b980-9b2e85c32a51",
					UserName = "Kaloqn",
					NormalizedUserName = "MUSICFAN@URBAN.COM",
					Email = "musicfan@urban.com",
					NormalizedEmail = "MUSICFAN@URBAN.COM",
					EmailConfirmed = true,
					PreferredTheme = "Light",
					PasswordHash = hasher.HashPassword(null, "MusicFan123!")
				},
				new UrbanUser
				{
					Id = "c5859895-19f2-47da-ae19-569400ee20d5",
					UserName = "Mr.Yanev",
					NormalizedUserName = "FASHIONGURU@URBAN.COM",
					Email = "fashionguru@urban.com",
					NormalizedEmail = "FASHIONGURU@URBAN.COM",
					EmailConfirmed = true,
					PreferredTheme = "Dark",
					PasswordHash = hasher.HashPassword(null, "Fashion123!")
				}
			};
			return users;
		}
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UrbanUser>>();

            string adminEmail = "admin@urban.com";
            string adminPassword = "Admin123!";

            // 1. Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // 2. Create admin user if not exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new UrbanUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            // 3. Assign Admin role
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

    }
}
