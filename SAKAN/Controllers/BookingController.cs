using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAKAN.Constants;
using SAKAN.DTO;
using SAKAN.Models;
using SAKAN.Services;
using System.Threading.Tasks;

namespace SAKAN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly BookingRepo bookingRepo;
        private readonly RoomRepo roomRepo;

        private SendData _sendData = new SendData();
        public BookingController(UserManager<ApplicationUser> userManager,BookingRepo bookingRepo,RoomRepo roomRepo)
        {
            this.userManager = userManager;
            this.bookingRepo = bookingRepo;
            this.roomRepo = roomRepo;
        }

        [Authorize(Roles =Role.Owner)]
        [HttpPut("AddStudentInRoomByEmail")]
        public async Task<IActionResult> AddStudentInRoomByEmail(int roomId,string email,string ownerId)
        {
            int emptyPlaces=roomRepo.GetEmptyPlacesIRoom(roomId);
            if (emptyPlaces == 0)
            {
                
                _sendData.Message = "الغرفه ليس بها مكان فارغ";
                return Conflict(_sendData);
            }
            ApplicationUser user = await userManager.FindByNameAsync(email);
            Student student=user as Student;
            if (student==null)
            { 
                _sendData.Message = "برجاء ادخال ايميل صحيح لطالب";
                return BadRequest(_sendData);

            }
            bookingRepo.AddStudentInRoomByEmail(roomId, user.Id,ownerId);
            _sendData.Message = "تم اضافة الطالب الي الغرفه بنجاح";
            return Ok(_sendData);

        }

        [Authorize(Roles =Role.Student)]
        [HttpPost("BookingRequest")]
        public IActionResult BookingRequest (BookingDTO bookingDTO)
        {
            if(ModelState.IsValid)
            {
                int res= bookingRepo.BookingRequest(bookingDTO);
                if (res<0)
                {
                    return BadRequest(_sendData);
                }
                else
                {
                    _sendData.Message = "تم اضافة الطلب ";
                    return Accepted(_sendData);   

                }
            }
            
            return BadRequest(_sendData);
        }

        [Authorize(Roles =Role.Owner)]
        [HttpPut("ConfirmRequest")]
        public IActionResult ConfirmRequest(int bookingId,string OwnerId)
        {
            Booking booking=bookingRepo.GetById(bookingId);
            if (booking==null)
            {
                _sendData.Message = "لا يوجد طلب حجز بهذا الرقم";
                return BadRequest(_sendData);
            }
            if(booking.OwnerId==OwnerId)
            {
                int emptyPlaces = roomRepo.GetEmptyPlacesIRoom(booking.RoomId);
                if (emptyPlaces == 0)
                {

                    _sendData.Message = "الغرفه ليس بها مكان فارغ";
                    return Conflict(_sendData);
                }
                
                var result = bookingRepo.ConfirmRequist(bookingId);
                if (result == 1)
                {

                    _sendData.Message = "تم تأكيد الحجز والطالب اضيف في الغرفة بنجاح";
                    return Ok(_sendData);

                }

            }
            else
            {
                _sendData.Message = "الحجز لا يخص صاحب السكن";
                return BadRequest(_sendData);
            }
            return BadRequest(_sendData);   

        }

        [Authorize(Roles =Role.Student)]
        [HttpDelete("StudentCancelRequist")]
        public IActionResult CancelRequist(int id)
        {
            if (bookingRepo.IsRequistConfirmed(id))
            {
                
                _sendData.Message = "تم تأكيد الحجز لا يمكنك الغاء الحجز";
                return Conflict(_sendData);


            }
            int res=bookingRepo.CanselRequist(id);
            if (res > 0)
            {
                
                _sendData.Message = "تم الغاء طلب الحجز بنجاح";
                return Ok(_sendData);
            }
            return BadRequest(_sendData);
                

        }

        [Authorize(Roles = Role.Owner)]
        [HttpDelete("OwnerDeleteStudentRequist")]
        public IActionResult OwnerDeleteStudentRequist(int id)
        {
            if (bookingRepo.IsRequistConfirmed(id))
            {

                _sendData.Message = "تم تأكيد الحجز لا يمكنك حذف الطلب الحجز";
                return Conflict(_sendData);


            }
            int res = bookingRepo.CanselRequist(id);
            if (res > 0)
            {

                _sendData.Message = "تم حذف طلب الحجز بنجاح";
                return Ok(_sendData);
            }
            return BadRequest(_sendData);


        }

        [Authorize(Roles =Role.Owner)]
        [HttpDelete("EndBooking")]
        public IActionResult EndBooking(int id)
        {
             int  res=bookingRepo.EndBooking(id);
            if (res > 0)
            {
                
                _sendData.Message = "تم انهاء الحجز";
                return Ok(_sendData);
            }
            return BadRequest(_sendData);
            
        }

        [Authorize(Roles = Role.Owner)]
        [HttpGet("GetAllConfirmedBookings")]
        public IActionResult GetAllConfirmedBookings(string OwnerId)
        {
            var bookings =bookingRepo.GetBookings(OwnerId, true);
            if (bookings == null)
            {
                _sendData.Message = "لا يوجد حجوزات ";
                return Ok(_sendData);
            }
            _sendData.Message = "هذه كل الحجوزات المؤكده";
            _sendData.Data = bookings;
            return Ok(_sendData);
        }

        [Authorize(Roles = Role.Owner)]
        [HttpGet("GetNotConfirmedBookings")]
        public IActionResult GetNotConfirmedBookings(string OwnerId)
        {
            var bookings = bookingRepo.GetBookings(OwnerId,false);
            if(bookings == null)
            {
                _sendData.Message = "لا يوجد طلبات حجز ";
                return Ok(_sendData);
            }
            _sendData.Message = "هذه كل طلبات الحجز الغير مؤكده";
            _sendData.Data = bookings;
            return Ok(_sendData);
        }

        [Authorize(Roles = Role.Student)]
        [HttpGet("GetAllBookingForStudent")]
        public IActionResult GetAllRequistsAndBookings(string studentId)
        {
            var bookings = bookingRepo.GetAllRequistsAndBookings(studentId);
            if (bookings == null)
            {
                _sendData.Message = "لا يوجد طلبات حجز ";
                return Ok(_sendData);
            }
            _sendData.Message = "هذه كل طلبات الحجز";
            _sendData.Data = bookings;
            return Ok(_sendData);

        }


    }
}
