using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAKAN.DTO;
using SAKAN.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAKAN.Services
{
    public class BookingRepo
    {
        private readonly SakanEntity context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public BookingRepo(SakanEntity context, UserManager<ApplicationUser> userManager,IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public int AddStudentInRoomByEmail(int roomId, string studentId,string ownerId)
        {
            Booking booking = new Booking();
            booking.RoomId = roomId;
            booking.StudentId = studentId;
            booking.OwnerId = ownerId;
            booking.Confirmed = true;
            context.Booking.Add(booking);

            AddStudentInRoom(roomId, studentId);
            return context.SaveChanges();
            
        }
        


        public int BookingRequest(BookingDTO bookingDTO)
        {
            Booking booking = mapper.Map<Booking>(bookingDTO);
            context.Booking.Add(booking);
            context.SaveChanges();
            return booking.Id;
        }

        public Booking GetById(int id)
        {
            Booking booking=context.Booking.Where(b=>b.Id == id).FirstOrDefault();
            return booking;
        }

        public int ConfirmRequist(int  id)
        {
            Booking booking =context.Booking.Where(b=>b.Id == id).FirstOrDefault();
            booking.Confirmed = true;
            AddStudentInRoom(booking.RoomId,booking.StudentId);
            context.SaveChanges();
            return 1;
        }

        public bool IsRequistConfirmed(int id)
        {
            Booking booking=context.Booking.Where(b=> b.Id == id).FirstOrDefault();
            return booking.Confirmed;
        }
        public int  CanselRequist(int id)
        {
            Booking booking = context.Booking.FirstOrDefault(b => b.Id == id);
            if (booking != null)
            {
                context.Booking.Remove(booking);
                return context.SaveChanges();
            }
            return 0;
        }

        public int EndBooking (int BookingId)
        {
            Booking booking = context.Booking.FirstOrDefault(b=>b.Id==BookingId);
            if (booking != null)
            {
                context.Booking.Remove(booking);
                removeStudentFromRoom(booking.RoomId, booking.StudentId);
                context.SaveChanges();
                return 1;
            }
        
            return 0;
        }
        

        public ICollection<BookingDetailsDTO> GetBookings(string ownerId,bool confermed)
        {
            ICollection<Booking> bookings = context.Booking.Where(b => b.OwnerId == ownerId && b.Confirmed == confermed).ToList();
            if (bookings.Count == 0)
                return null;
            ICollection<BookingDetailsDTO> bookingDetails = new List<BookingDetailsDTO>();
            BookingDetailsDTO bookingDetail = new BookingDetailsDTO();
            foreach (var booking in bookings)
            {
                bookingDetail = mapper.Map<BookingDetailsDTO>(booking);
                bookingDetails.Add(bookingDetail);
            }
            return bookingDetails;
        }

        public ICollection<BookingDetailsDTO> GetAllRequistsAndBookings(string studentId)
        {
            ICollection<Booking> bookings = context.Booking.Where(b => b.StudentId== studentId).ToList();
            if (bookings.Count == 0)
                return null;
            ICollection<BookingDetailsDTO> bookingDetails = new List<BookingDetailsDTO>();
            BookingDetailsDTO bookingDetail = new BookingDetailsDTO();
            foreach (var booking in bookings)
            {
                bookingDetail = mapper.Map<BookingDetailsDTO>(booking);
                bookingDetails.Add(bookingDetail);
            }
            return bookingDetails;
        }
        private int AddStudentInRoom(int roomId, string studentId)
        {
            StudentInRoom studentInRoom = new StudentInRoom();
            studentInRoom.RoomId = roomId;
            studentInRoom.StuedntId = studentId;
            context.StudentInRoom.Add(studentInRoom);
            var room=context.Room.Where(r => r.Id == roomId).FirstOrDefault();
            room.CurrentState++;
            return context.SaveChanges();

        }

        private int removeStudentFromRoom(int roomId,string studentId)
        {
            var studentInRoom= context.StudentInRoom.FirstOrDefault(i=>i.RoomId==roomId&& i.StuedntId==studentId);
            context.StudentInRoom.Remove(studentInRoom);
            var room = context.Room.Where(r => r.Id == roomId).FirstOrDefault();
            room.CurrentState--;
            return context.SaveChanges();
        }

    }
}
