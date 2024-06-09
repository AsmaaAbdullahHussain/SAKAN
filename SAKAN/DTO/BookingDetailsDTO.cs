using SAKAN.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAKAN.DTO
{
    public class BookingDetailsDTO
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string? OwnerId { get; set; }
        public int RoomId { get; set; }
        public bool Confirmed { get; set; } = false;
        public string Message { get; set; }
    }
}
