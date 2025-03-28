using Application.Interface;
using Application.Model.Assignment;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Application.App.Assignment.Command
{
    public class CreateAssignmentCommand : IRequest<object>
    {
        public CreateAssignmentDto createAssignmentDto { get; set; }
    }

    public class CreateAssignmentCommandHandler : IRequestHandler<CreateAssignmentCommand, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public CreateAssignmentCommandHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<object> Handle(CreateAssignmentCommand command, CancellationToken cancellationToken)
        {
            var assignmentform = command.createAssignmentDto;

            var test = new Domain.Enitities.Assignment
            {
                Title = assignmentform.TestTitle,
                Description = assignmentform.Description,
                testLink = "dfijgkmb",
                courseId = int.Parse(assignmentform.CourseId),
                DueDate = assignmentform.DueDate
            };
            await _appDbContext.Set<Domain.Enitities.Assignment>().AddAsync(test);
            await _appDbContext.SaveChangesAsync();

            foreach (var questionDTO in assignmentform.Questions)
            {
                var question = new Domain.Enitities.Questions
                {
                    AssignmentId = test.AssignmentId,
                    QuestionType = questionDTO.QuestionType,
                    QuestionText = questionDTO.QuestionText,
                };
                await _appDbContext.Set<Domain.Enitities.Questions>().AddAsync(question);
                await _appDbContext.SaveChangesAsync();
                foreach (var optiondto in questionDTO.Options)
                {
                    var option = new Domain.Enitities.Options
                    {
                        OptionText = optiondto,
                        QuestionId = question.QuestionId
                    };
                    await _appDbContext.Set<Domain.Enitities.Options>().AddAsync(option);
                    await _appDbContext.SaveChangesAsync();

                }
                var ans = new Domain.Enitities.Answer
                {
                    AnswerText = questionDTO.Answer,
                    QuestionId = question.QuestionId
                };
                await _appDbContext.Set<Domain.Enitities.Answer>().AddAsync(ans);
                await _appDbContext.SaveChangesAsync();

            }
            await _appDbContext.SaveChangesAsync();

            return new
            {
                status = 200,
                message = "Test Added"
            };
        }
    }
}

//*******************************************************************************************************

//public class TestService : ITestService
//{
//    private readonly ApplicationDbContext _dbContext;

//    public TestService(ApplicationDbContext dbContext)
//    {
//        _dbContext = dbContext;
//    }

//    public async Task<bool> CreateTestAsync(CreateTestDTO createTestDTO)
//    {
//        var test = new Test
//        {
//            TestTitle = createTestDTO.TestTitle,
//            Description = createTestDTO.Description,
//            CourseId = int.Parse(createTestDTO.CourseId),
//            DueDate = createTestDTO.DueDate,
//            Questions = new List<Question>()
//        };

//        // Add each question to the test
//        foreach (var questionDTO in createTestDTO.Questions)
//        {
//            var question = new Question
//            {
//                QuestionType = questionDTO.QuestionType,
//                QuestionText = questionDTO.QuestionText,
//                Answer = questionDTO.Answer,
//                Options = questionDTO.Options != null ? questionDTO.Options.Select(o => new Option { Text = o }).ToList() : null
//            };

//            test.Questions.Add(question);
//        }

//        // Add the test to the database
//        _dbContext.Tests.Add(test);
//        await _dbContext.SaveChangesAsync();
//        return true;
//    }
//}
