using Application.Model.Course;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation
{
    public class CourseValidator : AbstractValidator<AddCourseDto>
    {
        public CourseValidator() {

            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(4,20).WithMessage("Title should be between 4 and 20 characters.");


            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .Length(10, 500).WithMessage("Description should be between 10 and 500 characters.");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Category is required.")
                .MaximumLength(50).WithMessage("Category should not exceed 50 characters.");

            RuleFor(x => x.InstructorId)
                .NotEmpty().WithMessage("Instructor ID is required.")
                .GreaterThan(0).WithMessage("Instructor ID must be a valid positive number.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start Date is required.")
                .Must(BeAValidDate).WithMessage("Start Date must be a valid date.")
                .Must(date => DateTime.Parse(date) >= DateTime.Today)
                .WithMessage("Start Date cannot be earlier than today.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End Date is required.")
                .Must(BeAValidDate).WithMessage("End Date must be a valid date.")
                .Must((dto, endDate) => DateTime.Parse(endDate) >= DateTime.Today.AddDays(7))
                .WithMessage("End Date must be at least 7 days from today.");
        }
        private bool BeAValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }
    }
    
}





