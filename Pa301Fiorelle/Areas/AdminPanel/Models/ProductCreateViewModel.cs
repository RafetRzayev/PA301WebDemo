using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pa301Fiorelle.Areas.AdminPanel.Models
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; } = null!;
        public IFormFile Image { get; set; } = null!;
        public decimal Price { get; set; }
        public List<SelectListItem> Categories { get; set; } = [];
        public int CategoryId { get; set; }
    }
}
