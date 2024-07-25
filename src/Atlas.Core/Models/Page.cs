using Atlas.Core.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Atlas.Core.Models
{
    public class Page : ModelBase, IPermissionable
    {
        public int PageId { get; set; }
        public int Order { get; set; }

        [Required]
        public Category? Category { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(30)]
        public string? Icon { get; set; }

        [Required]
        [StringLength(50)]
        public string? Permission { get; set; }

        [Required]
        [StringLength(50)]
        public string? Route { get; set; }

        public bool IsPermitted(IEnumerable<string?> permissions)
        {
            ArgumentNullException.ThrowIfNull(permissions);

            return permissions.Contains(Permission);
        }
    }

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