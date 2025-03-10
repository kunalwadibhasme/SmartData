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
    public class CreateUserCommand : IRequest<object>
    {
        public required RegisterDto RegisterDto { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IGeneratePassword _generatePassword;
        private readonly IEmailService _emailService;

        public CreateUserCommandHandler(IAppDbContext appDbContext, IMapper mapper, IGeneratePassword generatePassword,
            IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _generatePassword = generatePassword;
            _emailService = emailService;
        }
        public async Task<object> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = command.RegisterDto;
            var existinguser = _appDbContext.Set<Domain.Enitities.User>().Where(u => u.Email == user.Email).FirstOrDefault();
            if (existinguser != null)
            {
                return new
                {
                    status = 400,
                    message = "User with this Email Id already Exist"
                };
            }
            var password = _generatePassword.GenerateRandomPassword(user.FirstName, user.LastName);
            var newuser = _mapper.Map<Domain.Enitities.User>(user);
            newuser.Password = BCrypt.Net.BCrypt.HashPassword(password);
            Guid key = Guid.NewGuid();
            var ApiKey = key.ToString();
            newuser.ApiKey = ApiKey;
            newuser.CreatedAt = DateTime.UtcNow;

            await _appDbContext.Set<Domain.Enitities.User>().AddAsync(newuser);
            await _appDbContext.SaveChangesAsync();
            var emailBody = $"{user.FirstName} {user.LastName}, you have successfully registered. " +
                                $"Your Password is {password}.";

            await _emailService.sendEmailAsync(user.Email, "Registeration Successfull", emailBody);
            return new
            {
                status = 200,
                message = "Registeration Successful!"
            };
        }
    }
}
