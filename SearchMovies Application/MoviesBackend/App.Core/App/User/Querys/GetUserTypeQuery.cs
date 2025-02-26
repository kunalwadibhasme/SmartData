using App.Core.App.Movies.Querys;
using App.Core.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.User.Querys
{
    public class GetUserTypeQuery : IRequest<object>
    {
    }

    public class GetUserTypeQueryHandler : IRequestHandler<GetUserTypeQuery, object>
    {
        private readonly IAppDbContext _appDbContext;
        public GetUserTypeQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<object> Handle(GetUserTypeQuery request, CancellationToken cancellationToken)
        {
            var usertype = await _appDbContext.Set<Domain.Entities.Usertype>().ToListAsync(cancellationToken);
            return new
            {
                status = 200,
                message = "List of Movies",
                Data = usertype
            };
        }
    }
}
