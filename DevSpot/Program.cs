using DevSpot.Data;
using DevSpot.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DevSpot.Repositories;
using DevSpot.Models;

namespace DevSpot
{
    public class Program
    {
        public static async Task Main(string[] args) // Change return type to Task
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
            });

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<IRepsoitory<JobPosting>, JobPostingRepository>(); // AddScope means that every time we use this line we will get the fresh new instance of the service that we are adding of every request 

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                await RoleSeeder.SeedRolesAsync(services);
                await UserSeeder.SeedUsersAsync(services); // what are services what are we passing to UserSeeder.SeedUserAsync function

            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=JobPostings}/{action=Index}/{id?}")
                .WithStaticAssets();

            await app.RunAsync(); // Use RunAsync for asynchronous execution
        }
    }
}
