using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Model
{
    public class RegisterationDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int UserTypeId { get; set; }
    }

    //public class RegisterationDtoValidator : AbstractValidator<RegisterationDto>
    //{
    //    public RegisterationDtoValidator()
    //    {
    //        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required.");
    //        RuleFor(x => x.FirstName).Length(3, 15).WithMessage("FirstName Should be 3 to 15 Characters long");

    //        RuleFor(x => x.LastName).NotEmpty().WithMessage("FirstName is required.");
    //        RuleFor(x => x.LastName).Length(3, 15).WithMessage("LastName Should be 3 to 15 Characters long");

    //        RuleFor(x => x.Email).EmailAddress().WithMessage("A valid email address is required.");
    //        RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required.");

    //        RuleFor(p => p.Password).NotEmpty().Length(8, 20);
    //        RuleFor(p => p.Password).Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.");
    //        RuleFor(p => p.Password).Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.");
    //        RuleFor(p => p.Password).Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
    //        RuleFor(x => x.Password).Matches(@"[\!\?\*\.]*$").WithMessage("Your password must contain at least one (!? *.).");
    //    }
    //}
}
