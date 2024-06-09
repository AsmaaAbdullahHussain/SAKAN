using Microsoft.AspNetCore.Http;
using SAKAN.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAKAN.DTO
{
    public class FlatDTO
    {
        public string Discreption { get; set; }

        [Required (ErrorMessage ="رقم الدور مطلوب ")]
        public short NumberOfFloor { get; set; }

        [Required(ErrorMessage ="عدد الغرف مطلوب")]
        public short NumberOfRooms { get; set; }
        public bool? ThereIsWasher { get; set; }

        
        public bool? ThereIsHeater { get; set; }

        public bool? TV { get; set; }
        public bool? Internet { get; set; }
        public short? NumberOfBathroom { get; set; }

        [Required]
        public int BuildingId { get; set; }

        
        
        public virtual ICollection<IFormFile> FlatImagesFile { get; set; }


        [MaxLength(20, ErrorMessage = "نوع الغساله لا يمكن ان يكون اكبر من 20 حرف")]
        public string WasherType { get; set; }
        

        [MaxLength(20, ErrorMessage = "نوع السخان لا يمكن ان يكون اكبر من 20 حرف")]
        public string HeaterType { get; set; }
        

    }
}
