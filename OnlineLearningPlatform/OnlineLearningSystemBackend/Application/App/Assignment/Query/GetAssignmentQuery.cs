using Application.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.App.Assignment.Query
{
    public class GetAssignmentQuery : IRequest<object>
    {
    }

    public class GetAssignmentQueryHandler : IRequestHandler<GetAssignmentQuery, object>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAssignmentQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(GetAssignmentQuery request, CancellationToken cancellationToken)
        {
            var result = await _appDbContext.Set<Domain.Enitities.Assignment>()
                .Include(a => a.Course) // Ensure Course is included
                .Select(a => new
                {
                    a.AssignmentId,
                    CourseTitle = a.Course != null ? a.Course.Title : "No Course", // Handle null Course
                    AssignmentTitle = a.Title,
                    a.Description,
                    a.testLink,
                    a.DueDate
                })
                .ToListAsync(cancellationToken); // Pass cancellationToken for better performance

            if (result.Any()) // More efficient than Count > 0
            {
                return new
                {
                    status = 200,
                    message = "Assignments Found",
                    data = result
                };
            }

            return new
            {
                status = 404,
                message = "Assignments Not Found"
            };
        }
    }
}
