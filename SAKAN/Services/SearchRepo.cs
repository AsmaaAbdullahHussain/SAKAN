using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAKAN.DTO;
using SAKAN.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAKAN.Services
{
    public class SearchRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SakanEntity _context;
        private readonly RoomRepo _roomRepo;

        public SearchRepo(UserManager<ApplicationUser> userManager, SakanEntity context,RoomRepo roomRepo)
        {
            _userManager = userManager;
            _context = context;
            _roomRepo = roomRepo;
        }
        public async Task<List<RoomCardDTO>> SearchRecommendedRoomsByAddressAsync(string studentId,string address)
        {
            var user = await _userManager.FindByIdAsync(studentId);
            Student student = user as Student;

            if (student == null) { return null; }

            var rooms = await _context.Room
                .Where(r => r.StudentInRoom.All(s => s.Student.Cluster == student.Cluster) && r.CurrentState < r.NumberOfBeds&& r.Flat.Building.Address== address)
                .Select(r => new RoomCardDTO
                {
                    Id = r.Id,
                    AirCondition = r.AirCondition,
                    NumberOfBeds = r.NumberOfBeds,
                    MonthPrice = r.MonthPrice,
                    Address = r.Flat.Building.Address,
                    UserGender = r.Flat.Building.UserGender
                })
                .ToListAsync();

            var empetyRooms = _context.Room.Where(r => r.CurrentState == 0&&r.Flat.Building.Address==address)
                .Select(r => new RoomCardDTO
                {
                    Id = r.Id,
                    AirCondition = r.AirCondition,
                    NumberOfBeds = r.NumberOfBeds,
                    MonthPrice = r.MonthPrice,
                    Address = r.Flat.Building.Address,
                    UserGender = r.Flat.Building.UserGender
                })
            .ToList();

            rooms.AddRange(empetyRooms);
            rooms = rooms.Where(r => r.UserGender == student.Gender).ToList();

            foreach (var room in rooms)
            {
                room.Images = _roomRepo.GetImagesLinks(room.Id);
            }

            return rooms;
        }

    }
}
