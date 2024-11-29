using Atlas.Core.Models;
using FluentValidation;

namespace Atlas.Core.Validation.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(v => v.Module)
                .NotNull().WithMessage("Module is required");

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(1, 20).WithMessage("Name cannot exceed 20 characters");

            RuleFor(v => v.Icon)
                .NotEmpty().WithMessage("Icon is required")
                .Length(1, 30).WithMessage("Icon cannot exceed 30 characters");

            RuleFor(v => v.Permission)
                .NotEmpty().WithMessage("Permission is required")
                .Length(1, 20).WithMessage("Permission cannot exceed 20 characters");
        }
    }
}
