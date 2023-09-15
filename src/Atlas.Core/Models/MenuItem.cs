using Atlas.Core.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Atlas.Core.Models
{
    public class MenuItem : ModelBase, IPermissionable
    {
        public int MenuItemId { get; set; }
        public int Order { get; set; }

        [Required]
        public Category? Category { get; set; }

        //[Required]   TODO remove comments....
        [StringLength(30)]
        public string? ComponentCode { get; set; }

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
        public string? NavigatePage { get; set; }

        public string NavigateFullPath()
        {
            return $@"{NavigatePage}\{ComponentCode}";
        }

        public bool IsPermitted(IEnumerable<string?> permissions)
        {
            if (permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }

            return permissions.Contains(Permission);
        }
    }

    public class MenuItemValidator : AbstractValidator<MenuItem>
    {
        public MenuItemValidator()
        {
            RuleFor(v => v.ComponentCode)
                .NotNull().WithMessage("Component Code is required");

            RuleFor(v => v.Category)
                .NotNull().WithMessage("Category is required");

            RuleFor(v => v.Name)
                .NotNull().WithMessage("Name is required")
                .Length(1, 20).WithMessage("Name cannot exceed 20 characters");

            RuleFor(v => v.Icon)
                .NotNull().WithMessage("Icon is required")
                .Length(1, 30).WithMessage("Icon cannot exceed 30 characters");

            RuleFor(v => v.Permission)
                .NotNull().WithMessage("Permission is required")
                .Length(1, 20).WithMessage("Permission cannot exceed 20 characters");

            RuleFor(v => v.NavigatePage)
                .NotNull().WithMessage("Navigate Page is required")
                .Length(1, 50).WithMessage("Navigate Page cannot exceed 50 characters");
        }
    }
}