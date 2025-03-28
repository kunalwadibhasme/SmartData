using Application.Interface;
using Application.Model.Assignment;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.App.Assignment.Query
{
    public class GetAssignmentTestQuery : IRequest<object>
    {
        public int AssignmentId { get; set; }
    }

    public class GetAssignmentTestQueryHandler : IRequestHandler<GetAssignmentTestQuery, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public GetAssignmentTestQueryHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<object> Handle(GetAssignmentTestQuery command, CancellationToken cancellationToken)
        {
            var assignment = await _appDbContext.Set<Domain.Enitities.Assignment>()
                .Where(a => a.AssignmentId == command.AssignmentId)
                .Select(a => new AssignmentGetDto
                {
                    AssignmentId = a.AssignmentId,
                    AssignmentName = a.Title,
                    Questions = a.Questions
                        .Select(q => new QuestionDto
                        {
                            QuestionId = q.QuestionId,
                            QuestionText = q.QuestionText,
                            Options = q.Options
                                .Select(o => new OptionDto
                                {
                                    OptionId = o.OptionId,
                                    OptionText = o.OptionText
                                }).ToList(),
                            Answer = q.Answer != null ? new AnswerDto
                            {
                                AnswerId = q.Answer.AnswerId,
                                AnswerText = q.Answer.AnswerText
                            } : null // Directly map the single Answer object
                        }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);
            if(assignment!=null)
            {
                return new
                {
                    status = 200,
                    message = "Data Fetched Successfully",
                    data = assignment
                };
            }
            else
            {
                return new
                {
                    status = 404,
                    message = "Data Not Found",
                    data = assignment
                };
            }
            
        }
    }
}
