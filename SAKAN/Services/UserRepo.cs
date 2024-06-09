using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SAKAN.DTO;
using SAKAN.Models;
using System.Threading.Tasks;

namespace SAKAN.Services
{
    public class UserRepo
    {
        private readonly SakanEntity _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserRepo(SakanEntity context , UserManager<ApplicationUser> userManager,IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<int> EditOwnerProfileAsync(string id,EditOwnerProfileDTO editOwner)
        {

            var owner=await _userManager.FindByIdAsync(id);
            if (owner == null)
                return 0;
            owner.FirstName = editOwner.FirstName;
            owner.LastName = editOwner.LastName;
            owner.PhoneNumber = editOwner.PhoneNumber;
            var result = await _userManager.UpdateAsync(owner);
            if (result.Succeeded) return 1;
            return -1;
        }

        public async Task<int> EditStudentProfileAsync(string id, EditStudentProfileDTO editStudent)
        {

            var user = await _userManager.FindByIdAsync(id);
            Student student=user as Student;
            if (student == null)
                return 0;
            
            _mapper.Map(editStudent, student);

            var result = await _userManager.UpdateAsync(student);
            if (result.Succeeded) return 1;
            return -1;
        }

        public async  Task<int> GetStudentClusterAsync(string StudentId)
        {
            var user=await _userManager.FindByIdAsync(StudentId);
            Student student = user as Student;
            if (student == null) return 0;
            return student.Cluster;
            
        }
    }
}
