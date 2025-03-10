using Application.Interface;
using Application.Model.Course;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.App.Course.Query
{
    public class UpdateCourseQuery : IRequest<object>
    {
        public UpdateCourseDto Updatecoursedto { get; set; }
    }

    public class UpdateCourseQueryHandler : IRequestHandler<UpdateCourseQuery, object>
    {
        private readonly IAppDbContext _appDbContext;
        public UpdateCourseQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> Handle(UpdateCourseQuery command, CancellationToken cancellationToken)
        {
            var update = command.Updatecoursedto;
            var existingcourse = _appDbContext.Set<Domain.Enitities.Course>().Where(c => c.CourseId == update.CourseId).FirstOrDefault();
            if (existingcourse != null)
            {
                existingcourse.Title = update.Title;
                existingcourse.Description = update.Description;
                existingcourse.Category = update.Category;
                existingcourse.StartDate = update.StartDate;
                existingcourse.EndDate = update.EndDate;

                _appDbContext.Set<Domain.Enitities.Course>().Update(existingcourse);
                await _appDbContext.SaveChangesAsync();

                return new
                {
                    status = 200,
                    message = "Course Updated!",
                    data = existingcourse
                };
            }
            else
            {
                return new
                {
                    status = 400,
                    message = "Course did not found"
                };
            }
        }
    }
}
