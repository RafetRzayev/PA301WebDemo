using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pa301Fiorelle.Areas.AdminPanel.Models;
using Pa301Fiorelle.DataContext;

namespace Pa301Fiorelle.Areas.AdminPanel.Controllers
{
    public class ProductController : AdminBaseController
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _dbContext.Products
                .Include(x => x.Category)
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var model = await GetProductCreateViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = await GetProductCreateViewModel();

                return View(model);
            }
            var isExist = await _dbContext.Products.AnyAsync(p => p.Name.ToLower() == model.Name.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("", "This product already exists.");
                model = await GetProductCreateViewModel();
                return View(model);
            }
            var fileName = Path.GetFileNameWithoutExtension(model.Image.FileName) + Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
            
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }
            var product = new DataContext.Entities.Product
            {
                Name = model.Name,
                ImageName = fileName,
                Price = model.Price,
                CategoryId = model.CategoryId
            };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<ProductCreateViewModel> GetProductCreateViewModel()
        {
            var categories =  await _dbContext.Categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                .ToListAsync();

            return new ProductCreateViewModel { Categories = categories };
        }
    }
}