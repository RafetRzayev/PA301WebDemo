using System.ComponentModel.DataAnnotations;

namespace Pa301Fiorelle.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public string? ReturnUrl { get; set; }
    }
}
