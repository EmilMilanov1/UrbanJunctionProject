using UrbanJunction.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data.Models;

namespace UrbanJunction.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Database
            string connectionString = builder.Configuration
                .GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Services
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Identity
            builder.Services.AddDefaultIdentity<UrbanUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            await app.RunAsync();
        }

        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UrbanUser>>();

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