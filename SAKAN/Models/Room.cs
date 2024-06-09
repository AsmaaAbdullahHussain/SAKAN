using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAKAN.Models
{
    public class Room
    {
        public int Id { get; set; }
        
        public string Description { get; set; }
        public bool AirCondition { get; set; }

        [Required]
        public short NumberOfBeds { get; set; }
        public short CurrentState { get; set; } = 0;
        public short NumberOfDisks { get; set;}
        public short NumberOfChairs { get; set; }
        public short NumberOfCupboards { get; set; }

        [Required]
        [MaxLength(20)]
        public string  WindowType { get; set; }

        [Required]
        public short ServicesPrice { get; set; }

        [Required]
        public short InsurancePrice { get; set; }

        [Required]
        public short MonthPrice { get; set; }

        [Required]
        public short DayPrice { get; set; }

        [ForeignKey("Flat")]
        public int FlatId { get; set; }

        public virtual Flat Flat { get; set; }

        public virtual ICollection<RoomImage> RoomImages { get; set; }
        public virtual ICollection<Student> Students { get; set; }

        public ICollection<StudentInRoom> StudentInRoom { get; set; }



    }
}
