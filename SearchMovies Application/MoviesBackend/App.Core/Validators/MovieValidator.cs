using App.Core.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Validators
{
    public class MovieValidator : AbstractValidator<AddMovieDto>
    {
        public MovieValidator()
        {
            RuleFor(x => x.MovieName).NotEmpty().WithMessage("MovieName is required.");
            RuleFor(x => x.MovieName).Length(1, 30).WithMessage("MovieName Should be 3 to 30 Characters long");

            RuleFor(x => x.Year).NotEmpty().WithMessage("Year is Required");
            RuleFor(x => x.Year).Length(4).WithMessage("Year should have 4 digits");
        }

    }
}
