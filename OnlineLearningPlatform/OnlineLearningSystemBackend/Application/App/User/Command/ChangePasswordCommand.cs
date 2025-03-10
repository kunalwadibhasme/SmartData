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
    public class ChangePasswordCommand : IRequest<object>
    {
        public required LoginDto loginDto { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IGeneratePassword _generatePassword;
        private readonly IEmailService _emailService;

        public ChangePasswordCommandHandler(IAppDbContext appDbContext, IMapper mapper, IGeneratePassword generatePassword,
            IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _generatePassword = generatePassword;
            _emailService = emailService;
        }
        public async Task<object> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var user = command.loginDto;
            var existinguser = _appDbContext.Set<Domain.Enitities.User>().Where(u => u.Email == user.Email).FirstOrDefault();
            if (existinguser != null && BCrypt.Net.BCrypt.Verify(user.Password, existinguser.Password))
            {
                var password = _generatePassword.GenerateRandomPassword(existinguser.FirstName, existinguser.LastName);
                existinguser.Password = BCrypt.Net.BCrypt.HashPassword(password);
                _appDbContext.Set<Domain.Enitities.User>().Update(existinguser);
                await _appDbContext.SaveChangesAsync();
                var emailBody = $"{existinguser.FirstName} {existinguser.LastName}, your NewPassword is {password}";
                await _emailService.sendEmailAsync(user.Email, "New Password", emailBody);
                return new
                {
                    status = 200,
                    message = "New Password Sent To Your Mail"
                };
            }
            return new
            {
                status = 400,
                message = "Email or Old Password is InCorrect"
            };
        }
    }
}
