using System.Collections.Generic;

namespace SAKAN.DTO
{
    public class RoomSummaryDTO
    {
        public int Id { get; set; }
        public short NumberOfBeds { get; set; }
        public string WindowType { get; set; }
        public short MonthPrice { get; set; }
        public string Image { get; set; }
    }
}
