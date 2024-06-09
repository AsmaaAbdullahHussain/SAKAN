using AutoMapper;
using SAKAN.DTO;
using SAKAN.Models;

namespace SAKAN.Helpers
{
    public class ProfileMapper:Profile
    {
        public ProfileMapper() 
        {
            CreateMap<RoomDTO, Room>();
            CreateMap<BuildingDTO, Building>();
            CreateMap<Building, BuildingSummaryDTO>();
            CreateMap<Building, BuildingDetailsDTO>();
            CreateMap<FlatDTO, Flat>();
            CreateMap<Flat, FlatSummaryDTO>();
            CreateMap<Flat,FlatDetailsDTO>();
            CreateMap<RoomDTO, Room>();
            CreateMap<BookingDTO, Booking>();
            CreateMap<StudentRegisterDTO, Student>();
            CreateMap<OwnerRegisterDTO, Owner>();
            CreateMap<Room,RoomDetailsDTO>();
            CreateMap<Room, RoomSummaryDTO>();
            CreateMap<Booking, BookingDetailsDTO>();
            CreateMap<Room, RoomCardDTO>();
            CreateMap<Student,StudentProfileDTO>();
            CreateMap<Owner, OwnerProfileDTO>();
            CreateMap<Owner, EditOwnerProfileDTO>();
            CreateMap<EditStudentProfileDTO,Student>();

        }
    }
}
