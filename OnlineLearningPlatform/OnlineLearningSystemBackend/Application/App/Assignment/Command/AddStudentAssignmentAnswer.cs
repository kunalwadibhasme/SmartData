using Application.Interface;
using Application.Model.Assignment;
using AutoMapper;
using Domain.Enitities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.App.Assignment.Command
{
    public class AddStudentAssignmentAnswer : IRequest<object>
    {
        public StudentAssignmentAnswerDto StudentAssignmentAnswerDto { get; set; }
    }

    public class AddStudentAssignmentAnswerHandler : IRequestHandler<AddStudentAssignmentAnswer, object>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public AddStudentAssignmentAnswerHandler(IAppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<object> Handle(AddStudentAssignmentAnswer command, CancellationToken cancellationToken)
        {
            var data = command.StudentAssignmentAnswerDto;

            // Check if the student has already submitted the assignment
            var existing = await _appDbContext.Set<Domain.Enitities.StudentsAssignmentSubmitted>()
                .Where(a => a.studentId == data.StudentId && a.AssignmentId == data.AssignmentId)
                .FirstOrDefaultAsync();

            if (existing != null)
            {
                return new
                {
                    status = 403,
                    message = "Student has already submitted the test"
                };
            }

            // Create a new record for the student's assignment submission
            var submitted = new StudentsAssignmentSubmitted
            {
                studentId = data.StudentId,
                AssignmentId = data.AssignmentId
            };

            await _appDbContext.Set<Domain.Enitities.StudentsAssignmentSubmitted>().AddAsync(submitted);
            await _appDbContext.SaveChangesAsync();

            // Fetch all correct answers for the questions in the submitted answers
            var correctAnswers = await _appDbContext.Set<Domain.Enitities.Answer>()
                .Where(a => data.AssignmentAnswerDto.Answers.Keys.Contains(a.QuestionId))
                .ToListAsync();

            // Map the submitted answers and compare them with the correct answers
            var submittedAnswers = data.AssignmentAnswerDto.Answers
                .Select(answer =>
                {
                    // Get the correct answer for the question
                    var correctAnswer = correctAnswers.FirstOrDefault(a => a.QuestionId == answer.Key);

                    // Check if the submitted answer matches the correct answer
                    var isCorrect = correctAnswer != null && correctAnswer.AnswerText.Equals(answer.Value, StringComparison.OrdinalIgnoreCase);

                    // Create and return a new SubmittedAnswers object
                    return new SubmittedAnswers
                    {
                        AssignmentId = data.AssignmentId,
                        QuestionId = answer.Key,
                        AnswerText = answer.Value,
                        Correct = isCorrect // Set the Correct flag based on the comparison
                    };
                })
                .ToList();

            // Add the list of submitted answers to the database
            await _appDbContext.Set<Domain.Enitities.SubmittedAnswers>().AddRangeAsync(submittedAnswers);
            await _appDbContext.SaveChangesAsync();

            return new
            {
                status = 200,
                message = "Student assignment answers submitted successfully"
            };
        }
    }
}
