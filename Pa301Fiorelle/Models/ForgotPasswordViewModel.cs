using System.ComponentModel.DataAnnotations;

namespace Pa301Fiorelle.Models
{
    public class ForgotPasswordViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
    }
}
