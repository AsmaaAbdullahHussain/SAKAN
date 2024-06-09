using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SAKAN.Models
{
    public class RoomImage
    {
        [Key, Column(Order = 0)]
        public string Image { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("flat")]
        public int RoomId { get; set; }
        public Room room { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
