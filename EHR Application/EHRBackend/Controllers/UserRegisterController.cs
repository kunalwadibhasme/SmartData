using E_CommerceBackend.DTOs;
using E_CommerceBackend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegisterController : ControllerBase
    {
        private readonly RegisterService _registerService;
        public UserRegisterController(RegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterUser(RegisterDto user)
        {
            var result = await _registerService.RegisterUserAsync(user);
            if (result != null && result.Status != 500 && result.Status != 400)
            {
                return Ok(new
                {
                    statusCode = 200,
                    message = "User Registered Successfully"
                });
            }
            return Conflict(new
            {
                statusCode = 409,
                message = "Email already exists"
            });
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserDto user)
        {
            var result = await _registerService.UpdateUserAsync(user);
            return Ok(result);
        }



        [HttpPost("[action]")]
        public async Task<IActionResult> LoginUser(Logindto user)
        {
            var result = await _registerService.LoginUserAsync(user);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Verifyotp(OtpDto otpdto)
        {
            var result = await _registerService.VerifyOtpAsync(otpdto);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword(ChangePassworddto changepassworddto)
        {
            var result = await _registerService.ChangePasswordAsync(changepassworddto);
            return Ok(result);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var result = await _registerService.ForgetPasswordAsync(email);
            return Ok(result);
        }

        //[HttpGet("[action]")]
        //public async Task<ActionResult> GetUserById(int Id)
        //{
        //    var user = await _loginRegister.GetuserbyId(Id);
        //    return Ok(user);
        //}

        [HttpGet("[action]")]
        public async Task<ActionResult> UserType()
        {
            var type = await _registerService.GetUserType();
            return Ok(type);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> Gender()
        {
            var type = await _registerService.GetGender();
            return Ok(type);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> BloodGroup()
        {
            var type = await _registerService.GetBloodGroup();
            return Ok(type);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> Specialization()
        {
            var type = await _registerService.GetSpecialization();
            return Ok(type);
        }


        [HttpGet("[action]")]

        public async Task<ActionResult> CountryList()
        {
            var allCountry = await _registerService.GetCountry();
            return Ok(allCountry);
        }


        [HttpGet("[action]")]
        public async Task<ActionResult> StateList(int byCountryId)
        {
            var allState = await _registerService.GetState(byCountryId);
            return Ok(allState);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetUserById(int Id)
        {
            var user = await _registerService.GetuserbyId(Id);
            return Ok(user);
        }       
    }
}
