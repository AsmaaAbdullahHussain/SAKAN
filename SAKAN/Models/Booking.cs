using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAKAN.Models
{
    public class Booking
    {   
        public int Id { get; set; }
        [ForeignKey("Student")]
   
        public string StudentId { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey("Owner")]
        public string? OwnerId { get; set; }
        public virtual Owner Owner { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        public bool Confirmed { get; set; }=false;
        public string Message { get; set; }
    }
}
