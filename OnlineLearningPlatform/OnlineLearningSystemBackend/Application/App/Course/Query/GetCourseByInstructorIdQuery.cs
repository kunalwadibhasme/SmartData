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
    public class GetCourseByInstructorIdQuery :  IRequest<object>
    {
        public int Id { get; set; } 
    }

    public class GetCourseByInstructorIdQueryHandler : IRequestHandler<GetCourseByInstructorIdQuery, object>
    {
        public readonly IAppDbContext _appDbContext;

        public GetCourseByInstructorIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(GetCourseByInstructorIdQuery command, CancellationToken cancellationToken)
        {
            var result = await _appDbContext.Set<Domain.Enitities.Course>()
                   .Where(e => e.InstructorId == command.Id) // Filtering by UserId
               .ToListAsync(cancellationToken);

            if (result.Count > 0)
            {
                return new
                {
                    status = 200,
                    message = "Instructor's Courses",
                    Data = result
                };
            }
            else
            {
                return new
                {
                    status = 400,
                    message = "No Course created by Instructor",
                    Data = result
                };
            }

        }
    }
}
