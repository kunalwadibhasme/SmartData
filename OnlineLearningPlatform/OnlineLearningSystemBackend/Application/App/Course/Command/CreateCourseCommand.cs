using Application.Interface;
using Application.Model.Course;
using AutoMapper;
using MediatR;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.App.Course.Command
{
    public class CreateCourseCommand : IRequest<object>
    {
        public AddCourseDto AddCourse { get; set; }
    }
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CreateCourseCommandHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<object> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
        {
            var course = command.AddCourse;
            var existingcourse = _appDbContext.Set<Domain.Enitities.Course>().Where(c => c.Title == course.Title).FirstOrDefault();
            if (existingcourse != null)
            {
                return new
                {
                    status = 400,
                    message = "Course Already Exists"
                };
            }
            var newcourse = _mapper.Map<Domain.Enitities.Course>(course);
            await _appDbContext.Set<Domain.Enitities.Course>().AddAsync(newcourse);
            await _appDbContext.SaveChangesAsync();
            return new
            {
                status = 200,
                message= "Course Added Successfully!"
            };
        }
    }
}
