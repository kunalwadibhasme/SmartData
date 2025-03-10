using Application.App.User.Command;
using Application.App.User.Query;
using Application.Model;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearningApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            _logger.LogInformation("Register function is called for adding new User");
            var userId = await _mediator.Send(new CreateUserCommand { RegisterDto = registerDto });
            return Ok(userId);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(LoginDto loginDto)
        {
            _logger.LogInformation("Login function is called for adding new User");
            var userId = await _mediator.Send(new LoginUserCommand { login = loginDto });
            return Ok(userId);
        }


        [HttpPost("google-login")]
        public async Task<IActionResult> Googlelogin([FromBody] GoogleLoginDto googleLoginDto)
        {
            _logger.LogInformation("GoogleLogin function is called");
            var result = await _mediator.Send(new GoogleLoginCommand { googleLogin = googleLoginDto });
            return Ok(result);    
        }
        [HttpPost("facebook-login")]
        public async Task<IActionResult> Facebooklogin(string email)
        {
            _logger.LogInformation("GoogleLogin function is called");
            var result = await _mediator.Send(new FaceBookLoginCommand { email = email });
            return Ok(result);
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(LoginDto loginDto)
        {
            _logger.LogInformation("ChangePassword function is called for Changing Password");
            var userId = await _mediator.Send(new ChangePasswordCommand { loginDto = loginDto });
            return Ok(userId);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string Email)
        {
            _logger.LogInformation("ForgetPassword function is called for Changing Password");
            var userId = await _mediator.Send(new ForgetpasswordComand { Email = Email});
            return Ok(userId);
        }

        [HttpGet("Getprofile")]
        public async Task<IActionResult> GetUserProfile(int Id)
        {
            var result = await _mediator.Send(new GetprofileQuery { Id = Id });
            _logger.LogInformation("Getprofile " + result);
            return Ok(result);
        }


    }
}
