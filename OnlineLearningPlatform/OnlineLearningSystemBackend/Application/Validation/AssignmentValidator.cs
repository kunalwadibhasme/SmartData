using Application.Model.Assignment;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validation
{
    public class AssignmentValidator : AbstractValidator<AssignmentDto>
    {
        public AssignmentValidator() {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(4, 20).WithMessage("Title should be between 4 and 20 characters.");


            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .Length(10, 500).WithMessage("Description should be between 10 and 500 characters.");


            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Due date is required.")
                .Must(BeTodayOrFutureDate)
                .WithMessage("Due date must be today or a future date.");
        }
        private bool BeTodayOrFutureDate(string dueDateString)
        {
            if (DateTime.TryParse(dueDateString, out DateTime dueDate))
            {
                return dueDate.Date >= DateTime.Today;
            }
            return false; // Invalid date format
        }
    }
}
