using Microsoft.AspNetCore.Identity;

namespace Pa301Fiorelle.DataContext.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
