using Atlas.Core.Models;
using FluentValidation;

namespace Atlas.Core.Validation.Validators
{
    public class PageValidator : AbstractValidator<Page>
    {
        public PageValidator()
        {
            RuleFor(v => v.Category)
                .NotNull().WithMessage("Category is required");

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(1, 20).WithMessage("Name cannot exceed 20 characters");

            RuleFor(v => v.Icon)
                .NotEmpty().WithMessage("Icon is required")
                .Length(1, 30).WithMessage("Icon cannot exceed 30 characters");

            RuleFor(v => v.Permission)
                .NotEmpty().WithMessage("Permission is required")
                .Length(1, 20).WithMessage("Permission cannot exceed 20 characters");

            RuleFor(v => v.Route)
                .NotEmpty().WithMessage("Route is required")
                .Length(1, 50).WithMessage("Route cannot exceed 50 characters");
        }
    }
}
