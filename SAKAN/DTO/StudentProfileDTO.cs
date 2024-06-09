using SAKAN.Constants;
using System.ComponentModel.DataAnnotations;

namespace SAKAN.DTO
{
    public class StudentProfileDTO
    {
        
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        
        public Gender Gender { get; set; }
        public string Faculty { get; set; }
        public int Grade { get; set; }

        public string Address { get; set; }
        public int Age { get; set; }
    }
}
