using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pa301Fiorelle.Areas.AdminPanel.Data;
using Pa301Fiorelle.DataContext;
using Pa301Fiorelle.DataContext.Entities;

namespace Pa301Fiorelle
{
    public class Program
    {
        public static async Task Main(string[] args)
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

            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = "/Account/AccessDenied";
            //});

            // register email sender
            builder.Services.AddTransient<Services.IEmailSender, Pa301Fiorelle.Services.SmtpEmailSender>();
            // register Razor view to string renderer for generating email HTML from views
            builder.Services.AddScoped<Services.IViewRenderService, Services.RazorViewToStringRenderer>();


            builder.Services.AddSession();

            PathConstants.ProductPath = Path.Combine(builder.Environment.WebRootPath, "img");
            PathConstants.CategoryPath = Path.Combine(builder.Environment.WebRootPath, "img", "category");

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                await roleManager.CreateAsync(new IdentityRole("Member"));
                if (await userManager.FindByNameAsync("admin") is null)
                {
                    var admin = new AppUser
                    {
                        FullName = "Admin",
                        UserName = "admin",
                        Email = "admin@fiorelle.com",
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(admin, "Admin1234");
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
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
