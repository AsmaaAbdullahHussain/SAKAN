using System.ComponentModel.DataAnnotations.Schema;

namespace SAKAN.Models
{
    public class StudentInRoom
    {
        [ForeignKey("Student")]
        public string StuedntId { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
