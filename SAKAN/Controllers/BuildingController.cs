using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAKAN.Models;
using SAKAN.Services;
using System.Collections.Generic;
using SAKAN.DTO;
using Microsoft.AspNetCore.Authorization;
using SAKAN.Constants;
using AutoMapper;
using System.Threading.Tasks;

namespace SAKAN.Controllers
{
    [Authorize(Roles =Role.Owner)]
    [Route("api/[controller]")]
    [ApiController]
   
    public class BuildingController : ControllerBase
    {
        private readonly BuildingRepo buildingRepo;
        private readonly FileRepo fileRepo;
        private readonly IMapper mapper;
        private SendData sendData = new SendData();

        public BuildingController(BuildingRepo buildingRepo,FileRepo fileRepo,IMapper mapper)
        {
            this.buildingRepo = buildingRepo;
            this.fileRepo = fileRepo;
            this.mapper = mapper;

        }
        /// <binding>
        /// bind premitive type 1)Route segment /id..  2)query string ?id=..
        /// bind complex type from requist body
        /// </binding>

        
        [HttpPost("Add")]
        public IActionResult Add([FromForm]BuildingDTO buildingDTO)
        {
            
            if (!ModelState.IsValid)
            {
               
                sendData.Message = "برجاء ادخال بيانات صحيح’";
                return BadRequest(sendData);
            }
            if (buildingDTO.ImageFile != null)
            {
                #region save image
                var fileResult = fileRepo.SaveImage(buildingDTO.ImageFile);
                if (fileResult.Item1 == 1)
                {
                    buildingDTO.Image = fileResult.Item2; // getting name of image
                }
                #endregion

                
            }


            var productResult = buildingRepo.Add(buildingDTO);
            if (productResult > 0)
            {

                sendData.Message = "تم اضافة المبني بنجاح";
                sendData.Data = buildingRepo.GetAll(buildingDTO.OwnerId);
                return Ok(sendData);
            }
            else
            {

                sendData.Message = "حدثت مشكلة في الاضافه";
                return BadRequest(sendData);
            }
            
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromForm] BuildingDTO editBuilding)
        {
            if(ModelState.IsValid)
            {
                if (editBuilding.ImageFile != null)
                {
                    #region delete old image
                    string imageName = buildingRepo.GetImageName(id);
                    if(imageName!=null)
                        await fileRepo.DeleteImageAsync(imageName);
                    #endregion

                    #region save new image
                    if(editBuilding.ImageFile!= null)
                    {
                        var fileResult = fileRepo.SaveImage(editBuilding.ImageFile);
                        if (fileResult.Item1 == 1)
                        {
                            editBuilding.Image = fileResult.Item2; // getting unique name of image
                        }
                        #endregion
                        int result = buildingRepo.Edit(id, editBuilding);
                        if (result > 0)
                        {
                            sendData.Message = "تم تعديل البيانات بنجاح";
                            sendData.Data = buildingRepo.GetById(id);
                            return Ok(sendData);
                        }
                    }
                    
                }
            }
            return BadRequest();
        }

        [HttpGet("getbyid/{id:int}")] //api/building/getbyid/id
        public IActionResult GetById(int id) 
        { 
            var building= buildingRepo.GetById(id);
            if (building==null)
            {
                sendData.Message = "لا يوجد سكن بهذا الرقم";
                return BadRequest(sendData);
                
            }
            
            sendData.Data = building;
            return Ok(sendData);
        }

        [HttpGet("getall")]
        public IActionResult GetAll(string ownerId)//get all building for specific owner
        {
            var buildings= buildingRepo.GetAll(ownerId);
          
            sendData.Data = buildings;
            return Ok(sendData);
        }

        [HttpDelete]
        public async  Task<IActionResult> Delete(int id)
        {
            #region delete image from server
            string image =buildingRepo.GetImageName(id);
            if (image != null)
                await fileRepo.DeleteImageAsync(image);
            #endregion

            #region delete row from db
            int result =buildingRepo.Delete(id);
            #endregion
            if (result > 0)
            {
                sendData.Message = "تم حذف المبني والشقق و الغرف و الحجز الخاص بالمبني";
                return Ok(sendData);
            }
            return BadRequest();
        }

    }
}
