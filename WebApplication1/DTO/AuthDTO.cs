using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }

    public class LogiDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]        
        public string Password { get; set; }
    }
}
