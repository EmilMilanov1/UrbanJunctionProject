using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data;
using UrbanJunction.Data.Configuration;
using UrbanJunction.Data.Models;
using UrbanJunction.Data.Seeding;


namespace UrbanJunction.Web.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedUsersAsync(this WebApplication application)
        {
            using (var scope = application.Services.CreateScope())
            {
                UserManager<UrbanUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<UrbanUser>>();
                List<UserDTO> users = UrbanUserSeeder.GetUsers().ToList();

                foreach (var user in users)
                {
                    UrbanUser? provisionUser = await userManager.FindByEmailAsync(user.Email);

                    if (provisionUser == null)
                    {
                        provisionUser = new UrbanUser
                        {
                            Email = user.Email,
                            UserName = user.UserName,
                        };

                        await userManager.CreateAsync(provisionUser, user.Password);
                    }
                }
            }
            ;

            return application;
        }
        public static async Task<WebApplication> SeedPostsAsync(this WebApplication application)
        {
            using (var scope = application.Services.CreateScope())
            {
                UserManager<UrbanUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<UrbanUser>>();

                DbContextOptions<ApplicationDbContext> options = scope.ServiceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();

                ApplicationDbContext dbContext = new ApplicationDbContext(options);
                List<PostDTO> posts = PostSeeder.GetPosts().ToList();

                UrbanUser user = await userManager.FindByEmailAsync("artlover@urban.com")!;

                foreach (var post in posts)
                {
                    Post? provisionPost = await dbContext.Posts.FirstOrDefaultAsync(p => p.Title == post.Title);

                    if (provisionPost == null)
                    {
                        provisionPost = new Post
                        {
                            Title = post.Title,
                            Content = post.Content,
                            CreatedOn = post.CreatedOn,
                            SubcategoryId = post.SubcategoryId,
                            UserId = user.Id
                        };
                        await dbContext.Posts.AddAsync(provisionPost);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            ;

            return application;
        }
        public static async Task SeedAdminAsync(this WebApplication application)
        {
            using (var scope = application.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UrbanUser>>();

                // 1. Create Admin Role if it doesn't exist
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                // 2. Create Admin User if not exists
                var adminEmail = "admin@urban.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new UrbanUser
                    {
                        UserName = "AdminUser",  // ✅ Changed to avoid conflicts
                        Email = adminEmail,
                        EmailConfirmed = true,
                        ProfilePicturePath = "/images/default.jpg"
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin123!");

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Error creating admin: {error.Description}");
                        }
                        return;
                    }

                    // 3. Assign Admin Role to Admin User
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    // If user exists but doesn't have admin role, add it
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

            }
        }
    }
}
