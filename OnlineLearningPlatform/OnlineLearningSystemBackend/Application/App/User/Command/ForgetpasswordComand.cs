using Application.Interface;
using Application.Model;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.App.User.Command
{
    public class ForgetpasswordComand : IRequest<object>
    {
        public required string Email { get; set; }
    }

    public class ForgetpasswordComandHandler : IRequestHandler<ForgetpasswordComand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IGeneratePassword _generatePassword;
        private readonly IEmailService _emailService;

        public ForgetpasswordComandHandler(IAppDbContext appDbContext, IMapper mapper, IGeneratePassword generatePassword,
            IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _generatePassword = generatePassword;
            _emailService = emailService;
        }
        public async Task<object> Handle(ForgetpasswordComand command, CancellationToken cancellationToken)
        {
            var user = command.Email;
            var existinguser = _appDbContext.Set<Domain.Enitities.User>().Where(u => u.Email == user).FirstOrDefault();
            if (existinguser != null )
            {
                var password = _generatePassword.GenerateRandomPassword(existinguser.FirstName, existinguser.LastName);
                existinguser.Password = BCrypt.Net.BCrypt.HashPassword(password);
                _appDbContext.Set<Domain.Enitities.User>().Update(existinguser);
                await _appDbContext.SaveChangesAsync();
                var emailBody = $"{existinguser.FirstName} {existinguser.LastName}, your NewPassword is {password}";
                await _emailService.sendEmailAsync(user, "New Password", emailBody);
                return new
                {
                    status = 200,
                    message = "New Password Sent To Your Mail"
                };
            }
            return new
            {
                status = 400,
                message = "Email is Invalid or Does not Exists"
            };
        }
    }
}

