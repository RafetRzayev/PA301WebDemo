using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pa301Fiorelle.Areas.AdminPanel.Data;
using Pa301Fiorelle.Areas.AdminPanel.Models;
using Pa301Fiorelle.DataContext;
using Pa301Fiorelle.DataContext.Entities;

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
            var model = await GetCategories();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();

                return View(model);
            }

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("", "Please select an image file.");
                model.Categories = await GetCategories();
                return View(model);
            }

            if (!model.Image.IsValidSize(1))
            {
                ModelState.AddModelError("", "Image size must be less than 1MB.");
                model.Categories = await GetCategories();
                return View(model);
            }

            var isExist = await _dbContext.Products.AnyAsync(p => p.Name.ToLower() == model.Name.ToLower());
            if (isExist)
            {
                ModelState.AddModelError("", "This product already exists.");
                model.Categories = await GetCategories();
                return View(model);
            }
            var unicalFileName = await model.Image.SaveFileAsync(PathConstants.ProductPath);
            var product = new Product
            {
                Name = model.Name,
                ImageName = unicalFileName,
                Price = model.Price,
                CategoryId = model.CategoryId
            };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            var path = Path.Combine(PathConstants.ProductPath, product.ImageName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var model = new ProductUpdateViewModel
            {
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Categories = await _dbContext.Categories
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
                    .ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await GetCategories();
                return View(model);
            }

            var product = await _dbContext.Products.FindAsync(model.Id);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = model.Name;
            product.Price = model.Price;
            product.CategoryId = model.CategoryId;
            var oldImagePath = string.Empty;

            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("", "Please select an image file.");
                    model.Categories = await GetCategories();
                    return View(model);
                }
                if (!model.Image.IsValidSize(1))
                {
                    ModelState.AddModelError("", "Image size must be less than 1MB.");
                    model.Categories = await GetCategories();
                    return View(model);
                }
                var unicalFileName = await model.Image.SaveFileAsync(PathConstants.ProductPath);
                oldImagePath = Path.Combine(PathConstants.ProductPath, product.ImageName);
                product.ImageName = unicalFileName;

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();

            if (model.Image != null && System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<List<SelectListItem>> GetCategories()
        {
            var categories =  await _dbContext.Categories
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
                .ToListAsync();

            return categories;
        }
    }
}