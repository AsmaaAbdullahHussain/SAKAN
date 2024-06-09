using System.Collections.Generic;

namespace SAKAN.DTO
{
    public class RoomPageDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? AirCondition { get; set; }


        public short NumberOfBeds { get; set; }
        public short? NumberOfDisks { get; set; }
        public short? NumberOfChairs { get; set; }
        public short? NumberOfCupboards { get; set; }
        public string WindowType { get; set; }
        public short ServicesPrice { get; set; }
        public short InsurancePrice { get; set; }
        public short MonthPrice { get; set; }
        public short DayPrice { get; set; }
        public short CurrentState { get; set; }
        public ICollection<string> Images { get; set; }
        public FlatPageDTO Flat {  get; set; }
    }
}
