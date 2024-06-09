using System.ComponentModel.DataAnnotations;

namespace SAKAN.DTO
{
    public class EditOwnerProfileDTO
    {
        [Required(ErrorMessage = "مطلوب")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "مطلوب")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "مطلوب")]
        [Phone(ErrorMessage = "رقم التلفون غير صحيح")]
        public string PhoneNumber { get; set; }
    }
}
