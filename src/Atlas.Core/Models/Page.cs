using Atlas.Core.Interfaces;
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
}