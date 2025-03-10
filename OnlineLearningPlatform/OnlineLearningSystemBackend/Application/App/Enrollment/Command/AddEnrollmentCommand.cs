using Application.Interface;
using Application.Model.Enrollment;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.App.Enrollment.Command
{
    public class AddEnrollmentCommand : IRequest<object>
    {
        public EnrollmentDto Enrollment { get; set; }
    }
    public class AddEnrollmentCommandHandler : IRequestHandler<AddEnrollmentCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public AddEnrollmentCommandHandler(IMapper mapper, IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<object> Handle(AddEnrollmentCommand command, CancellationToken cancellationToken)
        {
            var enroll = command.Enrollment;
            var enrolled = await _appDbContext.Set<Domain.Enitities.Enrollments>().Where(c => c.CourseId == enroll.CourseId && c.StudentId == enroll.StudentId).FirstOrDefaultAsync();
            if (enrolled != null)
            {
                return new
                {
                    status = 400,
                    message = "You already registered in this course"
                };
            }
            var newenroll = _mapper.Map<Domain.Enitities.Enrollments>(enroll);
            newenroll.status = (Status)1;
            await _appDbContext.Set<Domain.Enitities.Enrollments>().AddAsync(newenroll);
            await _appDbContext.SaveChangesAsync();
            return new
            {
                status = 200,
                message = "You Enrolled to Course Successfully!"
            };
        }
    }
}
