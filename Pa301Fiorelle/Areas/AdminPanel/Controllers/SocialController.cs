using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pa301Fiorelle.DataContext;
using Pa301Fiorelle.DataContext.Entities;

namespace Pa301Fiorelle.Areas.AdminPanel.Controllers
{
    public class SocialController : AdminBaseController
    {
        private readonly AppDbContext _dbContext;

        public SocialController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var socials = await _dbContext.Socials.ToListAsync();

            return View(socials);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Social social)
        {
            if (!ModelState.IsValid)
            {
                return View(social);
            }

            var isExist = await _dbContext.Socials.AnyAsync(s => s.Name.ToLower() == social.Name.ToLower());

            if (isExist)
            {
                ModelState.AddModelError("", "This social media already exists.");
                return View(social);
            }

            await _dbContext.Socials.AddAsync(social);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var social = await _dbContext.Socials.FindAsync(id);
            if (social == null)
            {
                return NotFound();
            }
            _dbContext.Socials.Remove(social);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
