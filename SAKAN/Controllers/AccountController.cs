using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SAKAN.DTO;
using SAKAN.Models;
using System.Threading.Tasks;
using SAKAN.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata.Ecma335;
using SAKAN.Helpers;
using AutoMapper;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using SAKAN.Services;

namespace SAKAN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly UserRepo userRepo;
        SendData sendData = new SendData();

        public AccountController(UserManager<ApplicationUser> userManager ,IConfiguration configuration,IMapper mapper,UserRepo userRepo ) 
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.mapper = mapper;
            this.userRepo = userRepo;
        }
       
        //create account new Owner 
        [HttpPost("registerOwner")]
        public async Task<IActionResult> OwnerRegisteration(OwnerRegisterDTO OwnerRegisterDTO)
        {
            
           

            if (ModelState.IsValid)
            {

                Owner user = mapper.Map<Owner>(OwnerRegisterDTO);

              
                user.UserName = OwnerRegisterDTO.Email;
                
                

               IdentityResult result= await userManager.CreateAsync(user,OwnerRegisterDTO.ConfirmPassword);
                
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Role.Owner);

                    #region create and return Token
                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    claims.Add(new Claim(ClaimTypes.Name, user.Id));
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                    var roles = await userManager.GetRolesAsync(user);
                    string role = roles[0];

                    SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

                    SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    claims.Add(new Claim(ClaimTypes.Role, role));

                    var DurationInDays = Convert.ToDouble(configuration["JWT:DurationInDays"]);

                    JwtSecurityToken token = new JwtSecurityToken(
                       issuer: configuration["JWT:Issuer"],//web api url
                       audience: configuration["JWT:Audience"],//frontend url
                       claims: claims,
                       signingCredentials: signingCredentials,
                       expires: DateTime.Now.AddDays(DurationInDays)
                       );
                    string redyToken = new JwtSecurityTokenHandler().WriteToken(token);


                    sendData.Message = "تم التسجيل بنجاح";
                    sendData.Data = new { FirstName = user.FirstName, Token = redyToken, Role = role, ExpireOn = DateTime.Now.AddDays(DurationInDays), Id = user.Id };
                    return Ok(sendData);
                    
                    #endregion

                }
                else
                {
                    
                    sendData.Message = "الايميل بالفعل موجود";
                    return BadRequest(sendData);
                   
                }

            }

            return BadRequest(sendData);
            

            
        }

        //create account new student 
        [HttpPost("registerStudent")]// api/account/registerStudent
        public async Task<IActionResult> StudentRegisteration(StudentRegisterDTO userRegisterDTO)
        {
            if (ModelState.IsValid)
            {
                if(await userManager.FindByEmailAsync(userRegisterDTO.Email)!=null)
                {
                    
                    sendData.Message = "الايميل بالفعل مسجل";
                    return BadRequest(sendData);    
                }

                Student user = mapper.Map<Student>(userRegisterDTO);
                user.UserName = user.Email;

                IdentityResult result = await userManager.CreateAsync(user, userRegisterDTO.ConfirmPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Role.Student);
                    #region create and return Token
                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    claims.Add(new Claim(ClaimTypes.Name, user.Id));
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                    var roles = await userManager.GetRolesAsync(user);
                    string role = roles[0];

                    SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

                    SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    claims.Add(new Claim(ClaimTypes.Role, role));

                    var DurationInDays = Convert.ToDouble(configuration["JWT:DurationInDays"]);

                    JwtSecurityToken token = new JwtSecurityToken(
                       issuer: configuration["JWT:Issuer"],//web api url
                       audience: configuration["JWT:Audience"],//frontend url
                       claims: claims,
                       signingCredentials: signingCredentials,
                       expires: DateTime.Now.AddDays(DurationInDays)
                       );
                    string redyToken = new JwtSecurityTokenHandler().WriteToken(token);
                    
                    sendData.Message = "تم تسجيل الحساب بنجاح";
                    sendData.Data = new { Token = redyToken, Role = role, ExpireOn = DateTime.Now.AddDays(DurationInDays), Id = user.Id };

                    return Ok(sendData);
                    #endregion

                }
                else
                {
                    string errors = "";
                    foreach (var error in result.Errors)
                    {
                        errors = errors + "\n" + error.Description;
                    }
                    sendData.Message= errors;
                    return BadRequest(sendData);
                }

            }
           
            return BadRequest();
        }


        //check Account valid "login"  
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                SendData sendData = new SendData();
                #region check and create token
                ApplicationUser user =await userManager.FindByNameAsync(userDTO.Email);
                if (user != null)
                {
                   bool Result= await userManager.CheckPasswordAsync(user, userDTO.Password);
                    
                    if (Result)
                    {
                        #region create token
                        List<Claim> claims = new List<Claim>();

                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        claims.Add(new Claim(ClaimTypes.Name, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles = await userManager.GetRolesAsync(user);
                        string role = roles[0];

                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

                        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        claims.Add(new Claim(ClaimTypes.Role, role));

                        var DurationInDays =Convert.ToDouble(configuration["JWT:DurationInDays"]);

                        JwtSecurityToken token = new JwtSecurityToken(
                           issuer: configuration["JWT:Issuer"],//web api url
                           audience: configuration["JWT:Audience"],//frontend url
                           claims: claims,
                           signingCredentials: signingCredentials,
                           expires: DateTime.Now.AddDays(DurationInDays)
                           );
                        string redyToken= new JwtSecurityTokenHandler().WriteToken(token);
                        #endregion

                        sendData.Message = "تم تسجيل الدخول بنجاح";
                        sendData.Data = new { FirstName = user.FirstName, Token = redyToken, Role = role, ExpireOn = DateTime.Now.AddDays(DurationInDays), Id = user.Id };
                        return Ok(sendData);


                        
                    }
                    
                }
                sendData.Message = "الايميل او كلمة المرور خطأ";
                return BadRequest( sendData);
                #endregion

            }

            return BadRequest();
          
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            
            try
            {
                var user = await userManager.FindByIdAsync(changePasswordDTO.Id);
                if (user is null)
                {
                    sendData.Message = "لا يوجد مستخدم";
                    return BadRequest(sendData);
                }
                if (!await userManager.CheckPasswordAsync(user, changePasswordDTO.CurrentPassword))
                {
                    sendData.Message = "كلمة المرور الحاليه غير صحيحه";

                    return BadRequest(sendData);
                    
                }
                var result = await userManager.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);
                if (result.Succeeded)
                {
                    sendData.Message = "تم تغيير كلمة المرور بنجاح";
                    
                }
                else
                {
                    sendData.Message = "فشلت عملية تغيير كلمة المرور";

                    sendData.Data = result.Errors.Select(e => e.Description);
                }
                return Ok(sendData);
            }
            catch (Exception ex)
            {
                sendData.Message = "فشلت عملية تغيير كلمة المرور";
                sendData.Data = new List<string> { ex.Message };
                return BadRequest(sendData);
            }
        }

        [Authorize]
        [HttpGet("StudentProfile")]
        public async Task<IActionResult> GetStudentProfileAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            StudentProfileDTO profile = mapper.Map<StudentProfileDTO>(user);
            sendData.Data = profile;
            return Ok(sendData);
        }

        [Authorize]
        [HttpGet("OwnerProfile")]
        public async Task<IActionResult> GetOwnerProfileAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            OwnerProfileDTO profile = mapper.Map<OwnerProfileDTO>(user);
            sendData.Data = profile;
            return Ok(sendData);
        }

        [Authorize(Roles = Role.Owner)]
        [HttpPut("EditOwnerProfile")]
        public async Task<IActionResult> EditOwnerProfileAsync(string ownerId, [FromBody] EditOwnerProfileDTO ownerProfileDTO)
        {
            int ans =await userRepo.EditOwnerProfileAsync(ownerId, ownerProfileDTO);
            if (ans == 0)
            {
                sendData.Message = "لا يوجد صاحب سكن بهذا الرقم";
                return BadRequest(sendData);
            }
            if (ans == 1)
            {
                var user = await userManager.FindByIdAsync(ownerId);
                OwnerProfileDTO profile = mapper.Map<OwnerProfileDTO>(user);
                sendData.Data = profile;
                sendData.Message = "تم تعديل الملف الشخصي بنجاح";
                return Ok(sendData);
            }
            sendData.Message = "حدثت مشكله";
            return BadRequest(sendData);
            

        }

        [Authorize(Roles = Role.Student)]
        [HttpPut("EditStudentProfile")]
        public async Task<IActionResult> EditStudentProfileAsync(string studentId, [FromBody] EditStudentProfileDTO editStudent)
        {
            int ans = await userRepo.EditStudentProfileAsync(studentId, editStudent);
            if (ans == 0)
            {
                sendData.Message = "لا يوجد طالب بهذا الرقم";
                return BadRequest(sendData);
            }
            if (ans == 1)
            {
                var user = await userManager.FindByIdAsync(studentId);
                StudentProfileDTO profile = mapper.Map<StudentProfileDTO>(user);
                sendData.Data = profile;
                sendData.Message = "تم تعديل الملف الشخصي بنجاح";
                return Ok(sendData);
            }
            sendData.Message = "حدثت مشكله";
            return BadRequest(sendData);


        }





    }
}
