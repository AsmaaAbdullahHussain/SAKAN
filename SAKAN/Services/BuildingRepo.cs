using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SAKAN.DTO;
using SAKAN.Models;
using System.Collections.Generic;
using System.Linq;


namespace SAKAN.Services
{
    //services Building Model CRUD Operations
    public class BuildingRepo
    {
        /// <summary>
        /// read one =>GetById
        /// read all => GetAll
        /// add      
        /// Edit
        /// delete
        /// </summary>


        private readonly SakanEntity context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public BuildingRepo(SakanEntity context,IMapper mapper, IHttpContextAccessor httpContextAccessor)  //inject Context
        {
            this.context = context;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        
        public int Add(BuildingDTO buildingDTO)
        {

            
            Building building = mapper.Map<Building>(buildingDTO);


            context.Building.Add(building);
            return context.SaveChanges();

        }
        public List<BuildingSummaryDTO> GetAll(string OwnerId)
        {
            var buildings= context.Building.Where(building=>building.OwnerId == OwnerId).ToList();  
            List<BuildingSummaryDTO> buildingSummaries= new List<BuildingSummaryDTO>();
           
            foreach (var building in buildings)
            {
                BuildingSummaryDTO buildingSummary = mapper.Map<BuildingSummaryDTO>(building);
                if(building.Image != null)
                {
                    // Get the current HTTP context
                    HttpContext httpContext = httpContextAccessor.HttpContext;

                    // Get the server domain
                    string serverDomain = httpContext.Request.Host.Value;
                    buildingSummary.Image = serverDomain + "/images/" + building.Image;
                }
                
                buildingSummaries.Add(buildingSummary);
            }
            return buildingSummaries;
        }
        
        public BuildingDetailsDTO GetById(int id) 
        {
            Building building=context.Building.FirstOrDefault(b=>b.Id==id);
            if (building==null)
                return null;
            BuildingDetailsDTO buildingDetails = mapper.Map<BuildingDetailsDTO>(building);
            if (building.Image != null)
            {
                // Get the current HTTP context
                HttpContext httpContext = httpContextAccessor.HttpContext;

                // Get the server domain
                string serverDomain = httpContext.Request.Host.Value;
                buildingDetails.Image = serverDomain + "/images/" + building.Image;
            }
            return buildingDetails;
        }

        public int Edit(int id,BuildingDTO editBuilding)
        {
           Building building= context.Building.FirstOrDefault(b => b.Id == id);
            if (building!=null)
            {
                mapper.Map(editBuilding,building);
                
                return context.SaveChanges();
            }
            return -1;
            
        }

        public int Delete(int id)
        {
            Building building = context.Building.FirstOrDefault(b => b.Id == id);
            if (building!=null)
            {
                context.Building.Remove(building);
                return context.SaveChanges();
            }
            return 0;
        }
        public string GetImageName(int id)
        {
            Building building=context.Building.FirstOrDefault(b=>b.Id==id);
            return building!=null ? building.Image : null;
        }

    }
}
