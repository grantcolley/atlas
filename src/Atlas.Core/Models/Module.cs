using Atlas.Core.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Atlas.Core.Models
{
    public class Module : ModelBase, IPermissionable
    {
        public Module()
        {
            Categories = [];
        }

        public int ModuleId { get; set; }
        public int Order { get; set; }
        public List<Category> Categories { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(30)]
        public string? Icon { get; set; }

        [Required]
        [StringLength(50)]
        public string? Permission { get; set; }

        public bool IsPermitted(IEnumerable<string?> permissions)
        {
            ArgumentNullException.ThrowIfNull(permissions);

            if (permissions.Contains(Permission))
            {
                int count = Categories.Count;

                if (count > 0)
                {
                    for (int i = count - 1; i >= 0; i--)
                    {
                        if (Categories[i].IsPermitted(permissions))
                        {
                            continue;
                        }

                        Categories.RemoveAt(i);
                    }
                }

                return true;
            }
            else
            {
                Categories.Clear();
                return false;
            }
        }
    }

    public class ModuleValidator : AbstractValidator<Module>
    {
        public ModuleValidator()
        {
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
