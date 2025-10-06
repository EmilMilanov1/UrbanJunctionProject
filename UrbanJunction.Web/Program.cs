using UrbanJunction.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrbanJunction.Data.Models;

namespace UrbanJunction.Web
{
    public class Program
    {
        public static void Main(string[] args)
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
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;

                options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:Password:RequireDigit");
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();


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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                SeedAdminAsync(services);
            }
            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/Identity/Account/Register";
            //});

            app.UseStaticFiles();

            app.Run();


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
                    UserName = "admin@urban.com",
                    Email = "admin@urban.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, "Admin123!"); // or your own password
            }

            // 3. Assign Admin Role to Admin User
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

    }
}
