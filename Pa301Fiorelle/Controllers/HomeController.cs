using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pa301Fiorelle.DataContext;
using Pa301Fiorelle.Models;
using System.Diagnostics;

namespace Pa301Fiorelle.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _dbContext.Sliders.ToListAsync();
            var categories = await _dbContext.Categories.ToListAsync();
            var totalProducts = await _dbContext.Products.CountAsync();
            var products = await _dbContext.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Id)
                .Take(6)
                .ToListAsync();
            var bio = await _dbContext.Bios.SingleOrDefaultAsync();

            var homeViewModel = new HomeViewModel
            {
                Sliders = sliders,
                Categories = categories,
                Products = products,
                Bio = bio,
                TotalProducts = totalProducts
            };

            return View(homeViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LoadMoreProducts(int skip)
        {
            var products = await _dbContext.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Id)
                .Skip(skip)
                .Take(6)
                .ToListAsync();

            return PartialView("_ProductsPartial", products);
        }

        public IActionResult Details(int id)
        {
            ViewBag.Logo = "logo.png";

            return View();
        }
    }
}
