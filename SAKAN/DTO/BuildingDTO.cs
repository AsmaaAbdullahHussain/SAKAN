using Microsoft.AspNetCore.Http;
using SAKAN.Constants;
using System.ComponentModel.DataAnnotations;

namespace SAKAN.DTO
{
    public class BuildingDTO
    {
        [Required(ErrorMessage = "مطلوب")]
        public string Name { get; set; }

        [Required(ErrorMessage ="مطلوب")]
        public string Address { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public Gender UserGender { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public bool Gas { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public string OwnerId { get; set; }

        
        public IFormFile ImageFile { get; set; }

        public string Image { get; set; }
    }
}
