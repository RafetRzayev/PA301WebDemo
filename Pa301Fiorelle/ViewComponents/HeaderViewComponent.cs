using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pa301Fiorelle.DataContext;

namespace Pa301Fiorelle.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var bio = await _context.Bios.SingleOrDefaultAsync();

            return View(bio);
        }
    }

}
