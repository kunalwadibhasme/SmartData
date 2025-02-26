//using App.Core.App.User.Command;
//using App.Core.Interface;
//using AutoMapper;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace App.Core.App.Tokens.Command
//{
//    public class GenerateNewTokenAndRefreshToken : IRequest<object>
//    {
//        public required string Email { get; set; }
//    }
//    public class GenerateNewTokenAndRefreshTokenHandler : IRequestHandler<GenerateNewTokenAndRefreshToken, object>
//    {
//        private readonly IAppDbContext _appDbContext;
//        private readonly IMapper _mapper;
//        public GenerateNewTokenAndRefreshTokenHandler(IAppDbContext appDbContext, IMapper mapper)
//        {
//            _appDbContext = appDbContext;
//            _mapper = mapper;
//        }
//        public async Task<object> Handle(GenerateNewTokenAndRefreshTokenHandler command, CancellationToken cancellationToken)
//        {
//            var user = await _appDbContext.Set<Domain.Entities.Users>().SingleOrDefault(u => u.Email == command.);
//            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
//                return BadRequest("Invalid client request");
//            var newAccessToken = _jwtService.GenerateAccessToken(principal.Claims);
//            var newRefreshToken = _tokenService.GenerateRefreshToken();
//            user.RefreshToken = newRefreshToken;
//            _userContext.SaveChanges();
//            return Ok(new AuthenticatedResponse()
//            {
//                Token = newAccessToken,
//                RefreshToken = newRefreshToken
//            });
//        }
//    }
//}
