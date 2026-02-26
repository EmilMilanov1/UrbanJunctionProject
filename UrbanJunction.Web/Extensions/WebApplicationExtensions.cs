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
            using(var scope = application.Services.CreateScope())
            {
                UserManager<UrbanUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<UrbanUser>>();
                List<UserDTO> users = UrbanUserSeeder.GetUsers().ToList();

                foreach (var user in users) 
                {
                    UrbanUser? provisionUser = await userManager.FindByEmailAsync(user.Email);

                    if(provisionUser == null)
                    {
                        provisionUser = new UrbanUser
                        {
                            Email = user.Email,
                            UserName = user.UserName,
                        };

                        await userManager.CreateAsync(provisionUser, user.Password);
                    }
                }
            };

            return application;
        }
        public static async Task<WebApplication> SeedPostsAsync(this WebApplication application)
        {
            using(var scope = application.Services.CreateScope())
            {
                UserManager<UrbanUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<UrbanUser>>();

                DbContextOptions<ApplicationDbContext> options = scope.ServiceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();

                ApplicationDbContext dbContext = new ApplicationDbContext(options);
                List<PostDTO> posts = PostSeeder.GetPosts().ToList();

                UrbanUser user = await userManager.FindByEmailAsync("artlover@urban.com")!;

                foreach (var post in posts) 
                {
                    Post? provisionPost = await dbContext.Posts.FirstOrDefaultAsync(p => p.Title == post.Title);

                    if(provisionPost == null)
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
            };

            return application;
        }
    }
}
