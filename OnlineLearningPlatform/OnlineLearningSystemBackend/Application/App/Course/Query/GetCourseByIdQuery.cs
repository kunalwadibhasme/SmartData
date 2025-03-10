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
    public class GetCourseByIdQuery : IRequest<object>
    {
        public int Id { get; set; }
    }

    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, object>
    {
        private readonly IAppDbContext _appDbContext;
        public GetCourseByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext; 
        }

        public async Task<object> Handle(GetCourseByIdQuery command, CancellationToken cancellationToken)
        {
            var result = await _appDbContext.Set<Domain.Enitities.Course>()
                   .Where(e => e.CourseId == command.Id) // Filtering by UserId
               .ToListAsync(cancellationToken);

            if (result.Count > 0)
            {
                return new
                {
                    status = 200,
                    message = "Course by Id",
                    Data = result
                };
            }
            else
            {
                return new
                {
                    status = 400,
                    message = "No Course Found",
                    Data = result
                };
            }
        }
    }
}
