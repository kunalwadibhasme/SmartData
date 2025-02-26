using App.Core.App.User.Command;
using App.Core.App.User.Querys;
using App.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MovieController> _logger;

        public UserController(IMediator mediator, ILogger<MovieController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterationDto registrationDto)
        {
            _logger.LogInformation("Register function is called for adding new User");
            var userId = await _mediator.Send(new CreateUserCommand {  Registeration = registrationDto });
            return Ok(userId);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(LoginDto loginDto)
        {
            _logger.LogInformation("Login function is called");
            var login = await _mediator.Send(new LoginUserCommand {LoginDto = loginDto});
            return Ok(login);
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            _logger.LogInformation("Change Password function is called");
            var detail = await _mediator.Send(new ChangePasswordCommand { Changepassword = changePasswordDto });
            return Ok(detail);
        }

        [HttpGet("Usertype")]
        public async Task<IActionResult> Usertype()
        {
            _logger.LogInformation("Usertype function is called");
            var result = await _mediator.Send(new GetUserTypeQuery { });
            return Ok(result);

        }
    }
}
