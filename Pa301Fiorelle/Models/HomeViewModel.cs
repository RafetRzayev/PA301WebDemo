using Pa301Fiorelle.DataContext.Entities;

namespace Pa301Fiorelle.Models
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; } = [];
        public List<Category> Categories { get; set; } = [];
        public List<Product> Products { get; set; } = [];
        public Bio? Bio { get; set; }
    }
}
