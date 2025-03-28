using Application.Interface;
using Application.Model.Assignment;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.App.Assignment.Command
{
    public class AddNewAssignmentCommand : IRequest<object>
    {
        public AssignmentDto assignmentDto { get; set; }
    }
    public class AddNewAssignmentCommandHandler : IRequestHandler<AddNewAssignmentCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public AddNewAssignmentCommandHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<object> Handle(AddNewAssignmentCommand command, CancellationToken cancellationToken)
        {
            var assignment = command.assignmentDto;
            var assign = _appDbContext.Set<Domain.Enitities.Assignment>().Where(a => a.Title == assignment.Title).FirstOrDefault();
            if(assign!=null)
            {
                return new
                {
                    status = 403,
                    message = "Assignment with this title already exists"
                };
            }
            var newassignment = _mapper.Map<Domain.Enitities.Assignment>(assignment);
            await _appDbContext.Set<Domain.Enitities.Assignment>().AddAsync(newassignment);
            await _appDbContext.SaveChangesAsync();
            return new
            {
                status = 200,
                message = "Assignment Created"
            };
        }
    }
}
