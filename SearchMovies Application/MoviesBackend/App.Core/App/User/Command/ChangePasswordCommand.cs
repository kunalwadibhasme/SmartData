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
    public class ChangePasswordCommand : IRequest<object>
    {
        public ChangePasswordDto Changepassword { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, object>
    {
        private readonly IAppDbContext _appDbContext;

        public ChangePasswordCommandHandler(IAppDbContext appDbContext)
        {
           _appDbContext = appDbContext;
        }

        public async Task<object> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var dto = command.Changepassword;

            var user = await _appDbContext.Set<Domain.Entities.Users>().FirstOrDefaultAsync(u => u.Email == dto.Email, cancellationToken);
            if(user == null)
            {
                return new
                {
                    status = 404,
                    message = "User Not Found"
                };
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            _appDbContext.Set<Domain.Entities.Users>().Update(user);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return new
            {
                status = 200,
                message = "Password Changed Successfully"
            };
        }
    }
}
