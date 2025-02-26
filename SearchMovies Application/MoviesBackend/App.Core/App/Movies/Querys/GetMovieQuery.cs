using App.Core.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.App.Movies.Querys
{
    public class GetMovieQuery : IRequest<object>
    {
        public string search { get; set; }
    }

    public class GetMovieQueryHandler : IRequestHandler<GetMovieQuery, object>
    {
        private readonly IAppDbContext _appDbContext;
        public GetMovieQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<object> Handle(GetMovieQuery request, CancellationToken cancellationToken)
        {
            var movie = _appDbContext.Set<Domain.Entities.Movies>().Where(u => u.Deleted == false);

            if(!string.IsNullOrWhiteSpace(request.search))
            {
                movie = movie.Where(p => p.MovieName != null && EF.Functions.Like(p.MovieName, $"%{request.search}%"));
            }
            var allmovies = await movie.ToListAsync(cancellationToken);
            return new
            {
                status = 200,
                message = "List of Movies",
                Data = allmovies
            };
        }
    }
}



