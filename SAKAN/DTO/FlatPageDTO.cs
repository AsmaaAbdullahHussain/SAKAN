using System.Collections.Generic;

namespace SAKAN.DTO
{
    public class FlatPageDTO
    {
        public int Id { get; set; }
        public string Discreption { get; set; }
        public short NumberOfFloor { get; set; }
        public short NumberOfRooms { get; set; }
        public bool ThereIsWasher { get; set; }
        public string WasherType { get; set; }
        public bool ThereIsHeater { get; set; }
        public string HeaterType { get; set; }
        public bool TV { get; set; }
        public bool Internet { get; set; }
        public short NumberOfBathroom { get; set; }
        public ICollection<string> Images { get; set; }
        public BuildingPageDTO Building {  get; set; }
    }
}