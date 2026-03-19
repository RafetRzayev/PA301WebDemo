using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pa301Fiorelle.Areas.AdminPanel.Data;
using Pa301Fiorelle.DataContext;
using Pa301Fiorelle.DataContext.Entities;

namespace Pa301Fiorelle
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            });


            builder.Services.AddIdentity<AppUser, IdentityRole>(op =>
            {
                op.Password.RequiredLength = 4;
                op.Password.RequireNonAlphanumeric = false;
                op.Password.RequireDigit = true;
                op.Password.RequireLowercase = false;
                op.Password.RequireUppercase = false;
                op.SignIn.RequireConfirmedAccount = false;

                op.Lockout.AllowedForNewUsers = false;
                op.Lockout.MaxFailedAccessAttempts = 5;
                op.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                op.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


            builder.Services.AddSession();

            PathConstants.ProductPath = Path.Combine(builder.Environment.WebRootPath, "img");
            PathConstants.CategoryPath = Path.Combine(builder.Environment.WebRootPath, "img", "category");

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            //app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
