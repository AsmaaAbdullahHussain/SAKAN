using Microsoft.AspNetCore.Http;
using SAKAN.Constants;
using System.ComponentModel.DataAnnotations;

namespace SAKAN.DTO
{
    public class BuildingDetailsDTO
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
