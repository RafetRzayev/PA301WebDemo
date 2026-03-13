using Microsoft.EntityFrameworkCore;
using Pa301Fiorelle.Areas.AdminPanel.Data;
using Pa301Fiorelle.DataContext;

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

            PathConstants.ProductPath = Path.Combine(builder.Environment.WebRootPath, "img");
            PathConstants.CategoryPath = Path.Combine(builder.Environment.WebRootPath, "img", "category");

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

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
