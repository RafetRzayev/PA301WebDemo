using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pa301Fiorelle.DataContext;
using Pa301Fiorelle.DataContext.Entities;
using Pa301Fiorelle.Models;

namespace Pa301Fiorelle.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var socials = await _context.Socials.ToListAsync();
            var bio = await _context.Bios.SingleOrDefaultAsync();
            var viewModel = new FooterViewModel
            {
                Socials = socials,
                FooterImageName = bio?.FooterImageName
            };
            return View(viewModel);
        }
    }

}
