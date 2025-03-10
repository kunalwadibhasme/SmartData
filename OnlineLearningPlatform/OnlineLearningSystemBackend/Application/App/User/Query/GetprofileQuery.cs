using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Application.App.User.Query
{
    public class GetprofileQuery : IRequest<object>
    {
        public int Id { get; set; }
    }

    public class GetprofileQueryHandler : IRequestHandler<GetprofileQuery, object>
    {
        public IAppDbContext _appDbContext {  get; set; }
        public GetprofileQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(GetprofileQuery request, CancellationToken cancellationToken)
        {
            var UserId = request.Id;
            var result = _appDbContext.Set<Domain.Enitities.User>().Where(c => c.UserId == UserId).ToList();
            if(result == null)
            {
                return new
                {
                    status = 404,
                    message = "User not found"
                };
            }
            return new
            {
                status = 200,
                message = "User Found",
                data = result
            };
        }
    }
}
