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
    public class GetEnrolledCourses : IRequest<object>
    {
        public int UserId { get; set; }
    }

    public class GetEnrolledCoursesHandler : IRequestHandler<GetEnrolledCourses, object>
    {
        private readonly IAppDbContext _appDbContext;

        public GetEnrolledCoursesHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(GetEnrolledCourses request, CancellationToken cancellationToken)
        {
            var result = await _appDbContext.Set<Domain.Enitities.Course>()
                .Where(c => _appDbContext.Set<Domain.Enitities.Enrollments>()
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


