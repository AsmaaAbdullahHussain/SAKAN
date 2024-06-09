using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SAKAN.DTO
{
    public class LoginUserDTO
    {
        [Required(ErrorMessage = "مطلوب")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage ="مطلوب")]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
