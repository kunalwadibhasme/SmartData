using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Model
{
    public class LoginDto
    {
        public string Email { get; set; }
        public required string Password { get; set; }
       
    }
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator() 
        { 
            RuleFor(x =>x.Email).EmailAddress().WithMessage("A valid email address is required.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required.");
            RuleFor(p => p.Password).NotEmpty().Length(8, 20);
            RuleFor(p => p.Password).Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.");
            RuleFor(p => p.Password).Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.");
            RuleFor(p => p.Password).Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
            RuleFor(x => x.Password).Matches(@"[\!\?\*\.]*$").WithMessage("Your password must contain at least one (!? *.).");
        }
    }
}
