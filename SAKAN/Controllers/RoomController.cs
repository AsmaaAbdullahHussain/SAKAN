using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SAKAN.DTO;
using SAKAN.Services;
using SAKAN.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SAKAN.Constants;

namespace SAKAN.Controllers
{
    [Authorize(Roles = Role.Owner)]
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomRepo roomRepo;
        private readonly IMapper mapper;
        private readonly FileRepo fileRepo;
        private readonly FlatRepo flatRepo;
        private SendData sendData = new SendData();

        public RoomController(RoomRepo roomRepo,IMapper mapper,FileRepo fileRepo,FlatRepo flatRepo)
        {
            this.roomRepo = roomRepo;
            this.mapper = mapper;
            this.fileRepo = fileRepo;
            this.flatRepo = flatRepo;
        }

        [HttpPost("Add")]
        public IActionResult Add([FromForm] RoomDTO roomDTO)
        {

            int numberOfRoomsInFlat = flatRepo.GetNumberOfRooms(roomDTO.FlatId);
            int numberOfRoomsExisting=roomRepo.NumberOfRoomsExisting(roomDTO.FlatId);
            if (numberOfRoomsExisting == numberOfRoomsInFlat)
            {
               
                sendData.Message = "لا يمكن اضافه غرفه جديده الغرف في الشقة مكتملة ";
                return Conflict(sendData);
            }
            if (!ModelState.IsValid)
            {
                
                sendData.Message = "برجاء ادخال بيانات صحيح’";
                return BadRequest(sendData);
            }
            if (roomDTO.RoomImagesFiles != null)
            {
                List<RoomImage> roomImages = new List<RoomImage>();
                RoomImage roomImage =new RoomImage();
                int roomId = roomRepo.Add(roomDTO);
                int numberOfImages = 0;
                foreach (var imagefile in roomDTO.RoomImagesFiles)
                {
                    var fileResult = fileRepo.SaveImage(imagefile);
                    if (fileResult.Item1 == 1)
                    {
                        roomImage.RoomId = roomId;
                        roomImage.Image = fileResult.Item2; // getting name of image
                        numberOfImages += roomRepo.AddRoomImage(roomImage);

                    }
                }

                if (numberOfImages == roomDTO.RoomImagesFiles.Count)
                {
                    if (roomId > -1)
                    {
                        
                        sendData.Message = "تم اضافة الغرفه بنجاح";
                        sendData.Data = roomRepo.GetById(roomId);
                        return Ok(sendData);
                    }

                }
                else
                {
                    
                    sendData.Message = "حدثت مشكلة في الاضافه";
                    return BadRequest(sendData);
                }

            }
            int roomid = roomRepo.Add(roomDTO);
            sendData.Message = "تم اضافة الغرفه بنجاح";
            sendData.Data=roomRepo.GetById(roomid);
            return Ok(sendData);



        }

        [HttpGet]
        public IActionResult Get(int flatId)
        { 
            sendData.Data = roomRepo.GetAll(flatId);
            return Ok(sendData);
        }

        [HttpGet("getbyid/{id:int}")]
        public IActionResult GetByid(int id)
        {
            sendData.Data = roomRepo.GetById(id);
            if (sendData.Data == null)
            {
                sendData.Message = "لا يوجد غرفه بهذا الرقم";
                return BadRequest(sendData);
            }
            return Ok(sendData);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm]RoomDTO roomDTO,int id)
        {
            if (ModelState.IsValid)
            {
                int res=roomRepo.Edit(id,roomDTO);
                if (res == 0)
                {
                    sendData.Message = "حدثت مشكله ";
                    return BadRequest(sendData);
                }
                #region delete old images
                var images=roomRepo.GetRoomImagesName(id);
                if(images.Count!= 0)
                {
                    foreach (var image in images)
                    {
                        await fileRepo.DeleteImageAsync(image);
                    }
                }
                roomRepo.DeleteRoomImages(id);
                #endregion

                #region save new images
                if(roomDTO.RoomImagesFiles!=null)
                {
                    RoomImage roomImage = new RoomImage();
                    foreach (var imageFile in roomDTO.RoomImagesFiles)
                    {
                        var fileResult = fileRepo.SaveImage(imageFile);
                        if (fileResult.Item1 == 1)
                        {
                            roomImage.Image = fileResult.Item2; // getting unique name of image
                            roomImage.RoomId = id;
                            roomRepo.AddRoomImage(roomImage);
                        }
                    }
                }
               
                #endregion
                sendData.Message = "تم تعديل البيانات بنجاح";
                sendData.Data = roomRepo.GetById(id);
                return Ok(sendData);
            }
            return BadRequest();
        }

        
        [HttpDelete]
        public async Task<IActionResult> Delete(int RoomId)
        {
            #region delete images

            var images = roomRepo.GetRoomImagesName(RoomId);
            foreach (var image in images)
            {
                await fileRepo.DeleteImageAsync(image);
            }
            int res = roomRepo.DeleteRoomImages(RoomId);

            #endregion

            int state =roomRepo.Delete(RoomId);
            if (state != 0)
            {
                sendData.Message = "تم الحذف بنجاح";
                return Ok(sendData);
            }
            return BadRequest();
        }

        
    }
}
