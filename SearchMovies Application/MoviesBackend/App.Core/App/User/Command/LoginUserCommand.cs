using App.Core.Interface;
using App.Core.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.User.Command
{
    public class LoginUserCommand : IRequest<object>
    {
        public LoginDto LoginDto { get; set; }
    }
    public class LoginUserCommandHandle : IRequestHandler<LoginUserCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IJwtService _jwtService;

        public LoginUserCommandHandle(IAppDbContext appDbContext, IJwtService jwtService)
        {
            _appDbContext = appDbContext;
            _jwtService = jwtService;
        }
        public async Task<object> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var LoginModel = command.LoginDto;
            var user = await _appDbContext.Set<Domain.Entities.Users>().FirstOrDefaultAsync(u => u.Email == LoginModel.Email);
            if (user == null || !VerifyPassword(LoginModel.Password, user.Password))
            {
                return new
                {
                    status = 401,
                    message = "Invalid email or Password"
                };
            }
            var role = await _appDbContext.Set<Domain.Entities.Usertype>().FirstOrDefaultAsync(r => r.UserTypeId == user.UserTypeId);
            if (role == null)
            {
                return new
                {
                    status = 404,
                    message = "Role Not Found"
                };
            }

            // Generate JWT token for the user

            var Token = _jwtService.GenerateToken(user.UserId, user.Email, role.UserType, user.ApiKey);
            var refreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(60);
            _appDbContext.Set<Domain.Entities.Users>().Update(user);
            await _appDbContext.SaveChangesAsync(); 
            var response = new
            {
                status = 200,
                message = "Successfully Logined In",
                role = role,
                token =Token,
                refreshToken =refreshToken,
                
            };
            return response;
        }

        public bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
        }
    }
}
