using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pa301Fiorelle.DataContext;
using Pa301Fiorelle.DataContext.Entities;

namespace Pa301Fiorelle.Areas.AdminPanel.Controllers
{
    public class CategoryController : AdminBaseController
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories.ToListAsync();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var isExist = await _dbContext.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("", "This category already exists.");
                return View(category);
            }
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            // Check whether category has any related products. If so, prevent deletion.
            //var hasProducts = await _dbContext.Products.AnyAsync(p => p.CategoryId == id);
            //if (hasProducts)
            //{
            //    TempData["ErrorMessage"] = "Cannot delete category because it has related products.";
            //    return RedirectToAction(nameof(Index));
            //}

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var isExist = await _dbContext.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower() && c.Id != category.Id);
            if (isExist)
            {
                ModelState.AddModelError("", "This category already exists.");
                return View(category);
            }
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}