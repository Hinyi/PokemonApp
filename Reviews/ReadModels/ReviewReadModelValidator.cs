using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Reviews.Command.AddNewReview;

namespace Reviews.ReadModels
{
    internal class ReviewReadModelValidator : AbstractValidator<AddNewReviewCommand>
    {
        public ReviewReadModelValidator()
        {
            //RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(16);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(1).MaximumLength(160);
            RuleFor(x => x.Rating).GreaterThanOrEqualTo(0).LessThanOrEqualTo(10).WithMessage("Rating cannot be greater than 10 and less than 0");
            RuleFor(x => x.Name).NotEmpty().MaximumLength(4);
        }
    }
}
