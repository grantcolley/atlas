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
}
