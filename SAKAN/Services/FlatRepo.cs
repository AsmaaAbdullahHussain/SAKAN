using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SAKAN.DTO;
using SAKAN.Models;
using System.Collections.Generic;
using System.Linq;

namespace SAKAN.Services
{
    public class FlatRepo
    {
        private readonly SakanEntity context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FlatRepo(SakanEntity context,IMapper mapper, IHttpContextAccessor httpContextAccessor) 
        {
            this.context = context;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }
        public int Add(FlatDTO flatDTO)
        {
            Flat flat = mapper.Map<Flat>(flatDTO);
            
            context.Flat.Add(flat);
            context.SaveChanges();
            return flat.Id;


        }

        public int AddFlatImage(FlatImage flatImage)
        {
            
            
                context.FlatImage.Add(flatImage);
                return context.SaveChanges();
                
            
            
        }

        public int GetNumberOfRooms(int id)
        {
            Flat flat = context.Flat.Where(f => f.Id == id).FirstOrDefault();
            return flat.NumberOfRooms;
        }


        public ICollection<FlatSummaryDTO> GetAll(int buildingId)
        {
            var flats=context.Flat.Where(flat=>flat.BuildingId==buildingId).ToList();
            
            List<FlatSummaryDTO> flatSummaries = new List<FlatSummaryDTO>();
            // Get the current HTTP context
            HttpContext httpContext = httpContextAccessor.HttpContext;

            // Get the server domain
            string serverDomain = httpContext.Request.Host.Value;
            foreach (var flat in flats)
            {
                FlatSummaryDTO flatSummary = mapper.Map<FlatSummaryDTO>(flat);
                FlatImage flatimage= context.FlatImage.FirstOrDefault(i => i.FlatId == flat.Id);
                if(flatimage != null)
                    flatSummary.Image = serverDomain + "/images/" + flatimage.Image;

                flatSummaries.Add(flatSummary);
            }
            return flatSummaries;
        }

        public FlatDetailsDTO GetById(int id) 
        { 
            Flat flat=context.Flat.FirstOrDefault(flat => flat.Id == id);
            if (flat == null)
                return null;
            FlatDetailsDTO flatDetails=mapper.Map<FlatDetailsDTO>(flat);


            flatDetails.Images = GetImagesLinks(id);
            return flatDetails;
        }
        public List<string> GetImagesLinks(int flatId)
        {
            List<string> images = new List<string>();

            var flatImages = context.FlatImage
                .Where(i => i.FlatId == flatId)
                .ToList();

            // Get the current HTTP context
            HttpContext httpContext = httpContextAccessor.HttpContext;

            // Get the server domain
            string serverDomain = httpContext.Request.Host.Value;

            foreach (var flatimage in flatImages)
            {
                images.Add(serverDomain + "/images/" + flatimage.Image);
            }
            return images;
        }

        public int Delete(int id)
        {

            Flat flat=context.Flat.FirstOrDefault(flat=>flat.Id==id);
            if(flat != null)
            {
                context.Flat.Remove(flat);
                return context.SaveChanges();
            }
            return 0;
        }
        public int DeleteFlatImages(int FlatId)
        {
            var flatImages = context.FlatImage.Where(i => i.FlatId == FlatId);
            foreach (var flatimage in flatImages)
            {
                context.FlatImage.Remove(flatimage);
            }
            return context.SaveChanges();
        }

        public ICollection<string> GetFlatImagesName(int id)
        {
            var flatImages=context.FlatImage.Where(i=>i.FlatId==id).ToList();
            List<string> images=new List<string>();
            foreach(var image in flatImages)
            {
                images.Add(image.Image);

            }
            return images;
        }

        public int Edit(int id,FlatDTO editFlat)
        {
            Flat flat = context.Flat.FirstOrDefault(f => f.Id == id);
            if (flat != null)
            {
                mapper.Map(editFlat, flat);

                return context.SaveChanges();
            }
            return -1;
        }
    }
}
