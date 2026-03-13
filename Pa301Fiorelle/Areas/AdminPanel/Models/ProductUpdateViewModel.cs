using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pa301Fiorelle.Areas.AdminPanel.Models
{
    public class ProductUpdateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public IFormFile? Image { get; set; }
        public decimal Price { get; set; }
        public List<SelectListItem> Categories { get; set; } = [];
        public int CategoryId { get; set; }
    }
}
