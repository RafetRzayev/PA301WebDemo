using Pa301Fiorelle.DataContext.Entities;

namespace Pa301Fiorelle.Models
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; } = new List<Slider>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Product> Products { get; set; } = new List<Product>();
        public Bio? Bio { get; set; }
        public int TotalProducts { get; set; }
    }
}
