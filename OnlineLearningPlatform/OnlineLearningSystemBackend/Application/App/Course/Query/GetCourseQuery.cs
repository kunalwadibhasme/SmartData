using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.App.Course.Query
{
    public class GetCourseQuery : IRequest<object>
    {
        public string search { get; set; }
    }

    public class GetMovieQueryHandler : IRequestHandler<GetCourseQuery, object>
    {
        private readonly IAppDbContext _appDbContext;
        public GetMovieQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<object> Handle(GetCourseQuery request, CancellationToken cancellationToken)
        {
            var course = _appDbContext.Set<Domain.Enitities.Course>().AsQueryable(); 

            if (!string.IsNullOrWhiteSpace(request.search))
            {
                course = course.Where(p => p.Title != null && EF.Functions.Like(p.Title, $"%{request.search}%"));
            }
            else
            {
                course = _appDbContext.Set<Domain.Enitities.Course>();
            }
            var allmovies = await course.ToListAsync(cancellationToken);
            return new
            {
                status = 200,
                message = "List of Movies",
                Data = allmovies
            };
        }
    }
}

