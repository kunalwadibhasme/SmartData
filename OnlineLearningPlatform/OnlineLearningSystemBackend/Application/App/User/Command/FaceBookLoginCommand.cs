using Application.Interface;
using Application.Model;
using AutoMapper;
using Google.Apis.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.App.User.Command
{
    public class FaceBookLoginCommand : IRequest<object>
    {
        public required string email { get; set; }
    }

    public class FaceBookLoginCommandHandler : IRequestHandler<FaceBookLoginCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public FaceBookLoginCommandHandler(IAppDbContext appDbContext, IMapper mapper, IJwtService jwtService)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<object> Handle(FaceBookLoginCommand request, CancellationToken cancellationToken)
        {
            //here it checks that the user logining with google has is valid or not then if yes then we can fetch the Email of user from result, generate token and return it to frontend else return BadRequest
            var Email = request.email;
            var existinguser = await _appDbContext.Set<Domain.Enitities.User>().Where(u => u.Email == Email).FirstOrDefaultAsync();
            if (existinguser != null)
            {
                Console.WriteLine(existinguser.Role.ToString());
                var Token = _jwtService.GenerateToken(existinguser.UserId, existinguser.Email, existinguser.Role.ToString(), existinguser.ApiKey);
                var refreshToken = _jwtService.GenerateRefreshToken();
                existinguser.RefreshToken = refreshToken;
                existinguser.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(60);
                _appDbContext.Set<Domain.Enitities.User>().Update(existinguser);
                await _appDbContext.SaveChangesAsync();

                return new
                {
                    status = 200,
                    message = "Login Successful",
                    token = Token,
                    refreshtoken = refreshToken
                };
            }
            return
            //in place of this in real application we will generate the token and return it     
            new
            {
                status = 400,
                message = "Invalid Email or password"
            };
        }
    }
}
