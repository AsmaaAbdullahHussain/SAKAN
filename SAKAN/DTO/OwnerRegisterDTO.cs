using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SAKAN.DTO
{
    public class OwnerRegisterDTO
    {
        [Required(ErrorMessage = "مطلوب")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "مطلوب")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "مطلوب")]
        [EmailAddress(ErrorMessage = "الايميل غير صحيح")]
        public string Email { get; set; }


        [Required(ErrorMessage = "مطلوب")]
        [RegularExpression("^[0-9]{6,}",ErrorMessage ="كلمة السر 6 ارقام او اكثر")]
        public string Password { get; set; }



        [Required(ErrorMessage = "مطلوب")]
        [Compare("Password",ErrorMessage ="كلمة المرور التأكيديه لا تطابق كلمة المرور")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "مطلوب")]
        [Phone(ErrorMessage = "رقم التلفون غير صحيح")]
        public string PhoneNumber { get; set;}

        
    }
}
