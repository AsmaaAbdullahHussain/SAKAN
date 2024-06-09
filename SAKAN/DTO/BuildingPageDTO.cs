using SAKAN.Constants;

namespace SAKAN.DTO
{
    public class BuildingPageDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public Gender UserGender { get; set; }
        public bool Gas { get; set; }
        public string OwnerId { get; set; }
        public string Image { get; set; }
    }
}