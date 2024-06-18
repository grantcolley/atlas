using FluentValidation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Atlas.Core.Models
{
    public class Permission : ModelBase
    {
        public Permission()
        {
            Roles = [];
        }

        public int PermissionId { get; set; }
        public List<Role> Roles { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(30)]
        public string? Code { get; set; }

        [Required]
        [StringLength(150)]
        public string? Description { get; set; }
    }

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
