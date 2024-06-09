using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SAKAN.Constants;

namespace SAKAN.Models
{
    public class Student:ApplicationUser
    {
        [Required(ErrorMessage = "مطلوب")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [StringLength(50,ErrorMessage ="اسم الكليه لا يمكن ان يكون اكبر من 50 حرف")]
        public string Faculty { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [Range(1,7,ErrorMessage ="الصف من 1 الي 7")]
        public int Grade { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [StringLength(20)]
        public string Governorate { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [StringLength(20)]
        public string City { get; set; }



        [NotMapped]
        public string Address
        {
            get
            {
                return City + "/" + Governorate;
            }
        }

        [Required(ErrorMessage = "مطلوب")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Range(1, 5)]
        public short Cluster { get; set; }


        
        [NotMapped]
        public int Age
        {
            get {
                DateTime today = DateTime.Today;
                int age= today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age)) age--; // Adjust if the birthday hasn't occurred this year

                return age;
                
            }
        }
        

    }
}
