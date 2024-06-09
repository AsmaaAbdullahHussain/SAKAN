using System.ComponentModel.DataAnnotations;

namespace SAKAN.DTO
{
    public class ChangePasswordDTO
    {
        [Required]
        public string Id { get; set; }
        [Required(ErrorMessage = "كلمة المرور الحالية مطلوبه")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "كلمة المرور الجديده مطلوبه")]
        [RegularExpression("^[0-9]{6,}", ErrorMessage = "كلمة السر 6 ارقام او اكثر")]
        public string NewPassword { get; set; }
    }
}
