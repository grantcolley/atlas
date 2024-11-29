using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace Atlas.Core.Models
{
    public class User : ModelBase
    {
        public User()
        {
            Roles = [];
            RoleChecklist = [];
        }

        public int UserId { get; set; }
        public List<Role> Roles { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [NotMapped]
        public List<ChecklistItem> RoleChecklist { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<string?> RoleList
        {
            get
            {
                if (RoleChecklist == null)
                {
                    return [];
                }

                return RoleChecklist
                    .Where(r => r.IsChecked)
                    .Select(r => r.Name)
                    .Distinct()
                    .ToList();
            }
        }

        public List<string?> GetPermissions()
        {
            return Roles
                .SelectMany(r => r.Permissions.Select(p => p.Code))
                .ToList();
        }
    }
}
