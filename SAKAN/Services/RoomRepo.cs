using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAKAN.DTO;
using SAKAN.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAKAN.Services
{
    public class RoomRepo
    {
        private readonly SakanEntity context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RoomRepo(SakanEntity context,IMapper mapper, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        public int Add(RoomDTO roomDTO)
        {
            Room room=mapper.Map<Room>(roomDTO);
            context.Room.Add(room);
            context.SaveChanges();
            return room.Id;
        }

        public int AddRoomImage(RoomImage roomImage)
        {
            context.RoomImage.Add(roomImage);
            return context.SaveChanges();
        }

        public int NumberOfRoomsExisting(int flatId)
        {
            List<Room> rooms=context.Room.Where(r=>r.FlatId==flatId).ToList();
            return rooms.Count;
        }
        
        //public ICollection<Room> GetAllRoomsNotCompleted()
        //{ 
        //    List<RoomHomeDTO> homeRooms = new List<RoomHomeDTO>();
        //    RoomHomeDTO homeRoom = new RoomHomeDTO();
        //    List<Room> rooms=context.Room.ToList();

        //    // Get the current HTTP context
        //    HttpContext httpContext = httpContextAccessor.HttpContext;

        //    // Get the server domain
        //    string serverDomain = httpContext.Request.Host.Value;

        //    foreach (Room room in rooms)
        //    {
        //        if(GetEmptyPlacesIRoom(room.Id)>0)
        //        {
        //            homeRoom = mapper.Map<RoomHomeDTO>(room);

        //            RoomImage roomImage = context.RoomImage.FirstOrDefault(i => i.RoomId == room.Id);

        //            if (roomImage != null)
        //                homeRoom.Image = serverDomain + "/images/" + roomImage.Image;

        //            homeRooms.Add(homeRoom);
        //        }
        //    }
        //}
       
        public int GetEmptyPlacesIRoom(int? roomid)
        {
            var room=context.Room.Where(r=>r.Id==roomid).FirstOrDefault();
            return room.NumberOfBeds-room.CurrentState;
        }

        public int Edit(int  id,RoomDTO updatedRoom)
        {
            Room room=context.Room.FirstOrDefault(room=>room.Id==id);
            if(room==null)
                return 0;
            mapper.Map(updatedRoom, room);
            return context.SaveChanges() ;
        }
        public int Delete(int RoomId)
        {
            Room room =context.Room.FirstOrDefault(room=> room.Id== RoomId);
            if (room!=null)
            {
                context.Room.Remove(room);
                return context.SaveChanges();
            }
            return 0;
        }
        public int DeleteRoomImages(int roomId)
        {
            List<RoomImage> roomImages =context.RoomImage.Where(i=>i.RoomId==roomId).ToList();
            foreach (var roomImage in roomImages)
            {
                context.RoomImage.Remove(roomImage);

            }
            return context.SaveChanges();
        }

        public ICollection<string> GetRoomImagesName(int roomId)
        {
            List<RoomImage> roomImages = context.RoomImage.Where(i => i.RoomId == roomId).ToList();
            List<string> images = new List<string>();
            foreach (var image in roomImages)
            {
                images.Add(image.Image);

            }
            return images;
        }
        public ICollection<RoomSummaryDTO> GetAll(int flatID)
        {
            List<Room> rooms = context.Room.Where(r =>r.FlatId==flatID).ToList();
            
            List<RoomSummaryDTO> roomSummaries= new List<RoomSummaryDTO>();

            // Get the current HTTP context
            HttpContext httpContext = httpContextAccessor.HttpContext;

            // Get the server domain
            string serverDomain = httpContext.Request.Host.Value;

            foreach (var room in rooms)
            {
                RoomSummaryDTO roomSummary = mapper.Map<RoomSummaryDTO>(room);
                RoomImage roomImage=context.RoomImage.FirstOrDefault(i=>i.RoomId==room.Id);

                if (roomImage != null)
                    roomSummary.Image = serverDomain + "/images/" + roomImage.Image;

                roomSummaries.Add(roomSummary);
            }

            return roomSummaries;
        }
        public RoomDetailsDTO GetById(int roomId)
        {
            Room room= context.Room.FirstOrDefault(r=> r.Id== roomId);
            if (room==null)
                return null;
            RoomDetailsDTO roomDetailsDTO = mapper.Map<RoomDetailsDTO>(room);

            roomDetailsDTO.Images = GetImagesLinks(roomId);

            return roomDetailsDTO;



        }
        public List<string> GetImagesLinks(int roomId)
        {
            List<string> images = new List<string>();
            var roomImages = context.RoomImage.Where(i => i.RoomId == roomId).ToList();

            // Get the current HTTP context
            HttpContext httpContext = httpContextAccessor.HttpContext;

            // Get the server domain
            string serverDomain = httpContext.Request.Host.Value;

            foreach (var roomImage in roomImages)
            {
                images.Add(serverDomain + "/images/" + roomImage.Image);
            }
            return images;
        }


        //privat methods
        private int NumberOfBedsInRoom(int? roomid)
        {
            Room room = context.Room.Where(r => r.Id == roomid).FirstOrDefault();
            return room.NumberOfBeds;

        }
        private int NumberOfStudentsExistingInRoom(int? roomId)
        {
            var users = userManager.Users.ToList();
            List<Student> students = new List<Student>();
            foreach (var user in users)
            {
                Student student = user as Student;
                if (student != null)
                {
                    students.Add(student);
                }
            }
            List<Student> studentsInRoom = students;//.Where(s => s.RoomId == roomId).ToList();
            return studentsInRoom.Count;

        }
    }
}
