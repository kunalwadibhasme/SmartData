using App.Core.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.App.Movies.Querys
{
    public class GetAllMovieQuery : IRequest<object>
    {
    }

    public class GetMoviesRequestHandler : IRequestHandler<GetAllMovieQuery, object>
    {
        private readonly IAppDbContext _appDbContext;
        public GetMoviesRequestHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<object> Handle(GetAllMovieQuery request, CancellationToken cancellationToken)
        {
            var result = await _appDbContext.Set<Domain.Entities.Movies>().Where(u =>u.Deleted == false).ToListAsync(cancellationToken);
            return new
            {
                status = 200,
                message = "All Movies",
                Data = result
            };
        }
    }
}
