using System;
using System.ComponentModel.DataAnnotations;

namespace SAKAN.DTO
{
    public class BookingDTO
    {
        [Required(ErrorMessage = "مطلوب")]
        public string StudentId { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public string OwnerId { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public int RoomId { get; set; }
        public string Message { get; set; }

       
    }
}
