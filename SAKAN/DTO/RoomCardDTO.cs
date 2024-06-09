using SAKAN.Constants;
using System.Collections.Generic;

namespace SAKAN.DTO
{
    public class RoomCardDTO
    {
        public int Id { get; set; }
        public bool AirCondition { get; set; }
        public short NumberOfBeds { get; set; }
        public short MonthPrice { get; set; }
        public string Address { get; set; }
        public Gender UserGender { get; set; }
        public ICollection<string> Images { get; set; }


       
    }
}
