using E_CommerceBackend.DTOs;
using E_CommerceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginRegisterController : ControllerBase
    {
        private readonly LoginRegister _loginRegister;
        public LoginRegisterController(LoginRegister loginRegister)
        {
                _loginRegister = loginRegister;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterUser(RegisterDto user)
        {
            var result = await _loginRegister.RegisterUserAsync(user);
            if (result != null && result.Status != 500 && result.Status!=400)
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
        public async Task<IActionResult> UpdateUserAsync(RegisterDto user)
        {
            var result = await _loginRegister.UpdateUserAsync(user);
            return Ok(result);
        }



        [HttpPost("[action]")]
        public async Task<IActionResult> LoginUser(Logindto user)
        {
            var result = await _loginRegister.LoginUserAsync(user);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Verifyotp(OtpDto otpdto)
        {
            var result = await _loginRegister.VerifyOtpAsync(otpdto);
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword(ChangePassworddto changepassworddto)
        {
            var result = await _loginRegister.ChangePasswordAsync(changepassworddto);
            return Ok(result);
        }

        [HttpPut("[action]")] 
        public async Task<IActionResult> ForgetPassword(string email) 
        { 
            var result = await _loginRegister.ForgetPasswordAsync(email); 
            return Ok(result); 
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetUserById(int Id)
        {
            var user = await _loginRegister.GetuserbyId(Id);
            return Ok(user);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> UserType()
        {
            var type = await _loginRegister.GetUserType();
            return Ok(type);
        }

        [HttpGet("[action]")]
        
        public async Task<ActionResult> CountryList()
        {
            var allCountry = await _loginRegister.GetCountry();
            return Ok(allCountry);
        }


        [HttpGet("[action]")]
        public async Task<ActionResult> StateList(int byCountryId)
        {
            var allState = await _loginRegister.GetState(byCountryId);
            return Ok(allState);
        }


    }
}



