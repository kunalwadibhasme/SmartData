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
    public class GetAllCourseQuery : IRequest<object>
    {
        public int UserId { get; set; }
    }

    public class GetAllCourseQueryHandler : IRequestHandler<GetAllCourseQuery, object>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllCourseQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(GetAllCourseQuery request, CancellationToken cancellationToken)
        {
            var result = await _appDbContext.Set<Domain.Enitities.Course>()
                .Where(c => !_appDbContext.Set<Domain.Enitities.Enrollments>()
                    .Where(e => e.StudentId == request.UserId) // Filtering by UserId
                    .Select(e => e.CourseId)
                    .Contains(c.CourseId))
                .ToListAsync(cancellationToken);

            return new
            {
                status = 200,
                message = "Courses Enrolled by the Given Student",
                Data = result
            };
        }
    }


}

//We fetch all Course records where the CourseId is not in the Enrollment table.
//The Select(e => e.CourseId) extracts all CourseIds that exist in the Enrollment table.
//The Contains(c.CourseId) checks if the current course exists in that list, and !Contains(...) 
//filters out those that are already enrolled.
