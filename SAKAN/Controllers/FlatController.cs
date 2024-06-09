using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAKAN.Constants;
using SAKAN.DTO;
using SAKAN.Models;
using SAKAN.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SAKAN.Controllers
{
    [Authorize(Roles = Role.Owner)]
    [Route("api/[controller]")]
    [ApiController]
    public class FlatController : ControllerBase
    {
        private readonly FlatRepo flatRepo;
        private readonly FileRepo fileRepo;
        private readonly BuildingRepo buildingRepo;
        private SendData sendData = new SendData();

        public FlatController(FlatRepo flatRepo,FileRepo fileRepo,BuildingRepo buildingRepo)
        {
            this.flatRepo = flatRepo;
            this.fileRepo = fileRepo;
            this.buildingRepo = buildingRepo;
        }

   

        [HttpPost("Add")]
        public IActionResult Add([FromForm] FlatDTO flatDTO)
        {

            if (!ModelState.IsValid)
            {
                
                sendData.Message = "برجاء ادخال بيانات صحيحه’";
                return BadRequest(sendData);
            }
            if (flatDTO.FlatImagesFile != null)
            {
                List<FlatImage> flatImages = new List<FlatImage>();
                FlatImage flatImage = new FlatImage();
                int flatId = flatRepo.Add(flatDTO);
                int numberOfImages = 0;
                foreach (var imagefile in flatDTO.FlatImagesFile)
                {
                    var fileResult = fileRepo.SaveImage(imagefile);
                    if (fileResult.Item1 == 1)
                    {
                        flatImage.FlatId = flatId;
                        flatImage.Image = fileResult.Item2; // getting name of image
                        numberOfImages += flatRepo.AddFlatImage(flatImage);
                        
                    }
                }
                
                if (numberOfImages ==flatDTO.FlatImagesFile.Count)
                {
                    if (flatId > -1)
                    {
                        sendData.Message = "تم اضافة الشقة بنجاح";
                        sendData.Data=flatRepo.GetById(flatId);
                        return Ok(sendData);
                    } 
                }
                else
                { 
                    sendData.Message = "حدثت مشكلة في الاضافه";
                    return BadRequest(sendData);
                }
            }
            int flatid = flatRepo.Add(flatDTO);
            sendData.Message = "تم اضافة الشقة بنجاح";
            sendData.Data = flatRepo.GetById(flatid);
            return Ok(sendData);

        }

        

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            #region delete images

            var images= flatRepo.GetFlatImagesName(id);
            foreach (var image in images)
            {
                await fileRepo.DeleteImageAsync(image);
            }
            int res=flatRepo.DeleteFlatImages(id);

            #endregion

            int result = flatRepo.Delete(id);
            if (result > 0)
            {
                sendData.Message = "تم حذف الشقه بنجاح";
                return Ok(sendData);
            }
            return BadRequest();
        }


        [HttpGet]
        public IActionResult GetAll(int buildingId)
        {
           var building =buildingRepo.GetById(buildingId);
            if (building == null)
            {
                sendData.Message ="لا يوجد سكن بهذا الرقم";
                return BadRequest(sendData);
            }
            var buildings = flatRepo.GetAll(buildingId);
            if (buildings.Count==0)
                sendData.Message = "المبني ليس به شقق";
            sendData.Data = buildings;  
            return Ok(sendData);

        }

        [HttpGet("getbyid")]
        public IActionResult Get(int id) 
        {
            
            var flat = flatRepo.GetById(id);
            if(flat == null)
            {
                sendData.Message = "لا يوجد شقه بهذا الرقم";
            }
            sendData.Data = flat;
            return Ok(sendData);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(int id, [FromForm] FlatDTO editFlat)
        {
            if (ModelState.IsValid)
            {
                if (editFlat.FlatImagesFile.Count != 0l)
                {
                    #region delete old images
                    var imagesName = flatRepo.GetFlatImagesName(id);
                    if (imagesName.Count != 0)
                    {
                        foreach (var imageName in imagesName)
                        {
                            await fileRepo.DeleteImageAsync(imageName);
                        }
                    }
                    int res=flatRepo.DeleteFlatImages(id);

                        
                    #endregion

                    #region save new images
                    if (editFlat.FlatImagesFile!=null)
                    {
                        FlatImage flatImage = new FlatImage();
                        foreach (var imageFile in editFlat.FlatImagesFile)
                        {
                            var fileResult = fileRepo.SaveImage(imageFile);
                            if (fileResult.Item1 == 1)
                            {
                                flatImage.Image = fileResult.Item2; // getting unique name of image
                                flatImage.FlatId = id;
                                flatRepo.AddFlatImage(flatImage);
                            }
                        }

                    }

                    #endregion
                    int result = flatRepo.Edit(id, editFlat);
                    if (result > 0)
                    {
                        sendData.Message = "تم تعديل البيانات بنجاح";
                        sendData.Data = flatRepo.GetById(id);
                        return Ok(sendData);
                    }
                }
            }
            return BadRequest();
        }

    }
}
