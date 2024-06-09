using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAKAN.Models
{
    public class FlatImage
    {
        [Key, Column(Order = 0)]
        public string Image { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("flat")]
        public int FlatId { get; set; }
        public Flat flat { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
