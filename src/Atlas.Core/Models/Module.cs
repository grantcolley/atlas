﻿using Atlas.Core.Attributes;
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
            Categories = new List<Category>();
        }

        [ModelPropertyRenderAttribute(Component = "Atlas.Blazor.Shared.Components.Mud.Label`1, Atlas.Blazor.Shared", Label = "Model Id", Order = 1, Tooltip = "Model Id")]
        public int ModuleId { get; set; }

        public int Order { get; set; }

        public List<Category> Categories { get; set; }

        [Required]
        [StringLength(50)]
        [ModelPropertyRenderAttribute(Component = "Atlas.Blazor.Shared.Components.Mud.Text`1, Atlas.Blazor.Shared", Label = "Name", Order = 2, Tooltip = "Name")]
        public string? Name { get; set; }

        [Required]
        [StringLength(30)]
        [ModelPropertyRenderAttribute(Component = "Atlas.Blazor.Shared.Components.Mud.Text`1, Atlas.Blazor.Shared", Label = "Icon", Order = 4, Tooltip = "Icon")]
        public string? Icon { get; set; }

        [Required]
        [StringLength(50)]
        [ModelPropertyRenderAttribute(Component = "Atlas.Blazor.Shared.Components.Mud.Text`1, Atlas.Blazor.Shared", Label = "Permission", Order = 5, Tooltip = "Permission")]
        public string? Permission { get; set; }

        public bool IsPermitted(IEnumerable<string?> permissions)
        {
            if (permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }

            if(permissions.Contains(Permission))
            {
                var count = Categories.Count;

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
                .NotNull().WithMessage("Name is required")
                .Length(1, 20).WithMessage("Name cannot exceed 20 characters");

            RuleFor(v => v.Icon)
                .NotNull().WithMessage("Icon is required")
                .Length(1, 30).WithMessage("Icon cannot exceed 30 characters");

            RuleFor(v => v.Permission)
                .NotNull().WithMessage("Permission is required")
                .Length(1, 20).WithMessage("Permission cannot exceed 20 characters");
        }
    }
}
