using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAKAN.DTO;
using SAKAN.Services;
using System.Threading.Tasks;

namespace SAKAN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly SearchRepo _searchRepo;
        SendData _sendData=new SendData();
        public SearchController(SearchRepo searchRepo) 
        {
            _searchRepo = searchRepo;
        }

        [HttpGet("SearchRoomsRecommendedByAddress")]
        public async Task<IActionResult> SearchRoomsRecommendedByAddress(string studentId,string address)
        {
            _sendData.Data= await _searchRepo.SearchRecommendedRoomsByAddressAsync(studentId,address);
            if (_sendData.Data == null)
            {
                _sendData.Message = "لا يوجد غرف مطابقه للبحث";
            }
            return Ok(_sendData);
        }

    }
}
