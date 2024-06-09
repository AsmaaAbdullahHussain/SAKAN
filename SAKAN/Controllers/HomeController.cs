using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAKAN.DTO;
using SAKAN.Models;
using SAKAN.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAKAN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly SakanEntity _context;
        private readonly RoomRepo _roomRepo;
        private readonly FlatRepo _flatRepo;
        private readonly HomeRepo _homeRepo;
        private readonly UserRepo _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private  SendData sendData= new SendData();

        public HomeController(SakanEntity context,RoomRepo roomRepo,FlatRepo flatRepo,HomeRepo homeRepo,UserRepo userRepo, IHttpContextAccessor httpContextAccessor)
        {
            _context=context;
            _roomRepo=roomRepo;
            _flatRepo = flatRepo;
            _homeRepo = homeRepo;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("allRoomsRecommended")]
        public async Task<IActionResult> GetAllRoomsRecommended(string  studentId)
        {
            //int cluster =await _userRepo.GetStudentClusterAsync(studentId);
            //if (cluster == 0)
            //{
            //    return NotFound();
            //}
            

            var rooms=await _homeRepo.GetRecommendedRoomsAsync(studentId);
            sendData.Data = rooms;
            return Ok(sendData);
        }
        [HttpGet("{roomId}")]
        public async Task<IActionResult> GetRoomDetails(int roomId)
        {
            var roomDetails = await _context.Room
                .Where(r => r.Id == roomId)
                .Select(r => new RoomPageDTO
                {
                    Id = r.Id,
                    Description = r.Description,
                    AirCondition = r.AirCondition,
                    NumberOfBeds = r.NumberOfBeds,
                    NumberOfDisks = r.NumberOfDisks,
                    NumberOfChairs = r.NumberOfChairs,
                    NumberOfCupboards = r.NumberOfCupboards,
                    WindowType = r.WindowType,
                    ServicesPrice = r.ServicesPrice,
                    InsurancePrice = r.InsurancePrice,
                    MonthPrice = r.MonthPrice,
                    DayPrice = r.DayPrice,
                    CurrentState = r.CurrentState,
                    
                    Flat = new FlatPageDTO
                    {
                        Id = r.FlatId,
                        Discreption = r.Flat.Discreption,
                        NumberOfFloor = r.Flat.NumberOfFloor,
                        NumberOfRooms = r.Flat.NumberOfRooms,
                        ThereIsWasher = r.Flat.ThereIsWasher,
                        WasherType = r.Flat.WasherType,
                        ThereIsHeater = r.Flat.ThereIsHeater,
                        HeaterType = r.Flat.HeaterType,
                        TV = r.Flat.TV,
                        Internet = r.Flat.Internet,
                        NumberOfBathroom = r.Flat.NumberOfBathroom,
                        Building = new BuildingPageDTO
                        {
                            Id = r.Flat.BuildingId,
                            Address = r.Flat.Building.Address,
                            Name = r.Flat.Building.Name,
                            Description = r.Flat.Building.Description,
                            UserGender = r.Flat.Building.UserGender,
                            Gas = r.Flat.Building.Gas,
                            OwnerId = r.Flat.Building.OwnerId,
                            Image = r.Flat.Building.Image
                        }
                    }
                })
                .FirstOrDefaultAsync();

            if (roomDetails == null)
            {
                return NotFound();
            }


            // Get the current HTTP context
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            // Get the server domain
            string serverDomain = httpContext.Request.Host.Value;


            roomDetails.Flat.Building.Image = serverDomain + "/images/" + roomDetails.Flat.Building.Image;
            roomDetails.Images = _roomRepo.GetImagesLinks(roomId);
            roomDetails.Flat.Images=_flatRepo.GetImagesLinks(roomDetails.Flat.Id);
            sendData.Data = roomDetails;



            return Ok(sendData);
        }
    }
}
