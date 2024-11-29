using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}
