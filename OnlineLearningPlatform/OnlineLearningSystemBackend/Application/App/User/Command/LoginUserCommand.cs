using Application.Interface;
using Application.Model;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.App.User.Command
{
    public class LoginUserCommand : IRequest<object>
    {
        public required LoginDto login { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public LoginUserCommandHandler(IAppDbContext appDbContext, IMapper mapper, IJwtService jwtService)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<object> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var user = command.login;
            var existinguser = await _appDbContext.Set<Domain.Enitities.User>().Where(u => u.Email == user.Email).FirstOrDefaultAsync();
            if (existinguser != null && BCrypt.Net.BCrypt.Verify(user.Password, existinguser.Password))
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
            return new
            {
                status = 400,
                message = "Invalid Email or password"
            };
        }
    }
}
