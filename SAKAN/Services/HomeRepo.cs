using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SAKAN.DTO;
using SAKAN.Migrations;
using SAKAN.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAKAN.Services
{
    public class HomeRepo
    {
        private readonly RoomRepo _roomRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FlatRepo _flatRepo;
        private readonly BuildingRepo _buildingRepo;
        private readonly IMapper _mapper;
        private readonly SakanEntity _context;

        public HomeRepo(RoomRepo roomRepo,UserManager<ApplicationUser> userManager,FlatRepo flatRepo,BuildingRepo buildingRepo,IMapper mapper,SakanEntity context)
        {
            _roomRepo = roomRepo;
            _userManager = userManager;
            _flatRepo = flatRepo;
            _buildingRepo = buildingRepo;
            _mapper = mapper;
            _context = context;
        }

       


        public async Task<List<RoomCardDTO>> GetRecommendedRoomsAsync(string studentId)
        {
            var user = await _userManager.FindByIdAsync(studentId);
            Student student = user as Student;

            if (student == null) { return null; }

            var rooms = await _context.Room
                .Where(r => r.StudentInRoom.All(s => s.Student.Cluster == student.Cluster) &&r.CurrentState<r.NumberOfBeds)
                .Select(r => new RoomCardDTO
                {
                    Id = r.Id,
                    AirCondition = r.AirCondition,
                    NumberOfBeds = r.NumberOfBeds,
                    MonthPrice = r.MonthPrice,
                    Address = r.Flat.Building.Address,
                    UserGender=r.Flat.Building.UserGender
                })
                .ToListAsync();

            var empetyRooms=_context.Room.Where(r => r.CurrentState == 0)
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
                room.Images=_roomRepo.GetImagesLinks(room.Id);
            }

            return rooms;
        }


    }
}
