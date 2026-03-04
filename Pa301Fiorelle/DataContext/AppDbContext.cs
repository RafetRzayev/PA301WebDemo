using Microsoft.EntityFrameworkCore;
using Pa301Fiorelle.DataContext.Entities;

namespace Pa301Fiorelle.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products {  get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
