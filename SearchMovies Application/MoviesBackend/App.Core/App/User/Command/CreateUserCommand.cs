using App.Core.Interface;
using App.Core.Model;
using AutoMapper;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.User.Command
{
    public class CreateUserCommand : IRequest<object>
    {
        public required RegisterationDto Registeration { get; set; }
    }
    public class CreateUserCommandHandle : IRequestHandler<CreateUserCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public CreateUserCommandHandle(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<object> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = command.Registeration;
            var existinguser = await _appDbContext.Set<Domain.Entities.Users>().FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existinguser == null)
            {
                var newUser = _mapper.Map<Domain.Entities.Users>(user);
                //var newUser = user.Adapt<Domain.Entities.Users>();
                newUser.Password = HashData(user.Password);
                Guid key = Guid.NewGuid();
                var ApiKey = key.ToString();
                newUser.ApiKey = ApiKey;
              
                await _appDbContext.Set<Domain.Entities.Users>().AddAsync(newUser);
                await _appDbContext.SaveChangesAsync(cancellationToken);
                var response = new
                {
                    status = 200,
                    message = "User Added Successfully",
                    data = newUser
                };
                return response;
            }
     
            var responses = new
            {
                status = 409,
                Message = "user already exists"
            };
            return responses;
        }

        public string HashData(string data)
        {
            return BCrypt.Net.BCrypt.HashPassword(data);
        }
    }
}
       
    
