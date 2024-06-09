using Microsoft.AspNetCore.Http;
using SAKAN.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAKAN.Models
{
    public class Building
    {
        public int Id { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public Gender UserGender { get; set; }

        [Required]
        public bool Gas { get; set; }
        
        public string Image { get; set; }

        [ForeignKey("Owner")]
        public string OwnerId { get; set; }

        public virtual Owner Owner { get; set; }
        public virtual ICollection<Flat> Flats { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
