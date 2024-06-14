using FluentValidation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Atlas.Core.Models
{
    public class Role : ModelBase
    {
        public Role()
        {
            Users = [];
            Permissions = [];
            PermissionChecklist = [];
        }

        public int RoleId { get; set; }
        public List<User> Users { get; set; }
        public List<Permission> Permissions { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(150)]
        public string? Description { get; set; }

        [NotMapped]
        public List<ChecklistItem> PermissionChecklist { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<string?> PermissionList
        {
            get
            {
                if (PermissionChecklist == null)
                {
                    return new List<string?>();
                }

                return [.. PermissionChecklist
                    .Where(p => p.IsChecked)
                    .Select(r => r.Name)
                    .OrderBy(p => p)];
            }
        }

        [NotMapped]
        [JsonIgnore]
        public List<string?> UserList
        {
            get
            {
                if (Users == null)
                {
                    return [];
                }

                return [.. Users
                    .Where(u => u != null)
                    .Select(u => u.Name)
                    .OrderBy(e => e)];
            }
        }
    }

    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(v => v.Name)
                .NotNull().WithMessage("Name is required")
                .Length(1, 50).WithMessage("Name cannot exceed 50 characters");

            RuleFor(v => v.Description)
                .NotNull().WithMessage("Description is required")
                .Length(1, 150).WithMessage("Description cannot exceed 150 characters");
        }
    }
}
