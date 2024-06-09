using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAKAN.Models
{
    public class Flat
    {
        public int Id { get; set; }
        public string Discreption { get; set; }

        [Required]
        public short NumberOfFloor { get; set; }

        [Required]
        public short NumberOfRooms { get; set; }
        public bool ThereIsWasher { get; set; }

        [MaxLength(20)]
        public string  WasherType { get; set;}
        public bool ThereIsHeater { get; set; }

        [MaxLength(20)]
        public string HeaterType { get; set;}
        public bool TV { get; set;}
        public bool Internet { get; set;}
        public short NumberOfBathroom { get; set; }

        [ForeignKey("Building")]
        public int BuildingId { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
        public virtual Building Building { get; set; }
        public virtual ICollection<FlatImage> FlatImages { get; set; }

        
        
        

    }
}
