using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using SAKAN.Constants;
using System;

namespace SAKAN.DTO
{
    public class StudentRegisterDTO
    {
        [Required(ErrorMessage = "مطلوب")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "مطلوب")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "مطلوب")]
        [EmailAddress(ErrorMessage = "الايميل غير صحيح")]
        public string Email { get; set; }


        [Required(ErrorMessage = "مطلوب")]
        [PasswordPropertyText]
        [RegularExpression("^[0-9]{6,}", ErrorMessage = "كلمة السر 6 ارقام او اكثر")]
        public string Password { get; set; }


        [Required(ErrorMessage = "مطلوب")]
        [PasswordPropertyText]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "مطلوب")]
        [Phone(ErrorMessage ="رقم التلفون غير صحيح")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [StringLength(50,ErrorMessage = "اسم الكليه لا يمكن ان يكون اكبر من 50 حرف")]
        public string Faculty { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [Range(1, 7, ErrorMessage = "الصف من 1 الي 7")]
        public int Grade { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [StringLength(20,ErrorMessage = "اسم المحافظة لا يمكن ان يكون اكبر من 20 حرف")]
        public string Governorate { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [StringLength(20, ErrorMessage = "اسم المحافظة لا يمكن ان يكون اكبر من 20 حرف")]
        public string City { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

    }
}
