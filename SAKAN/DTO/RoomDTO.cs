using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace SAKAN.DTO
{
    public class RoomDTO
    {
        

        public string Description { get; set; }
        public bool? AirCondition { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public short NumberOfBeds { get; set; }
        public short? NumberOfDisks { get; set; }
        public short? NumberOfChairs { get; set; }
        public short? NumberOfCupboards { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [MaxLength(20, ErrorMessage = "نوع النافزه لا يمكن ان يكون اكبر من 20 حرف")]
        public string WindowType { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public short ServicesPrice { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public short InsurancePrice { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public short MonthPrice { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public short DayPrice { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public int FlatId { get; set; }
        public ICollection<IFormFile> RoomImagesFiles { get; set; }
    }
}
