using Pa301Fiorelle.DataContext.Entities;

namespace Pa301Fiorelle.Models
{
    public class FooterViewModel
    {
            public string? FooterImageName { get; set; }
            public List<Social> Socials { get; set; } = [];
    }
}
