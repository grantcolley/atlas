using Atlas.Core.Models;
using FluentValidation;

namespace Atlas.Core.Validation.Validators
{
    public class PermissionValidator : AbstractValidator<Permission>
    {
        public PermissionValidator()
        {
            RuleFor(v => v.Code)
                .NotEmpty().WithMessage("Code is required")
                .MaximumLength(50).WithMessage("Code cannot exceed 30 characters");

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");

            RuleFor(v => v.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(50).WithMessage("Description cannot exceed 150 characters");
        }
    }
}
