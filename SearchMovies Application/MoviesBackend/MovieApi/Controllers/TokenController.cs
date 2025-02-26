using App.Core.App.User.Command;
using App.Core.Interface;
using App.Core.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IJwtService _jwtService;
        private readonly IMediator _mediator;
        private readonly ILogger<MovieController> _logger;

        public TokenController(IAppDbContext appDbContext, IJwtService jwtService,
            IMediator mediator, ILogger<MovieController> logger)
        {
           _appDbContext = appDbContext;
            _jwtService = jwtService;
            _mediator = mediator;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;

            var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
            var principalClaims = principal.Claims.ToList();
            var emailprincipal = principalClaims[2];
            var email = emailprincipal.Value;

            _logger.LogInformation("New AccessToken and RefreshToken is being Generated");
            var user = _appDbContext.Set<Domain.Entities.Users>().SingleOrDefault(u => u.Email == email);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");
            var newAccessToken = _jwtService.GenerateToken(
                int.Parse(principalClaims[0].Value),
                principalClaims[2].Value,
                principalClaims[3].Value,
                principalClaims[4].Value
            );
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _appDbContext.SaveChangesAsync();
            return Ok(new
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });

        }
    }
}
