using Atlas.Core.Models;
using FluentValidation;

namespace Atlas.Core.Validation.Validators
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(1, 50).WithMessage("Name cannot exceed 50 characters");

            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("Description is required")
                .Length(1, 150).WithMessage("Description cannot exceed 150 characters");
        }
    }
}
