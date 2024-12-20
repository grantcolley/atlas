using System.Collections.Generic;

namespace Atlas.Core.Models
{
    public class Authorisation
    {
        public Authorisation()
        {
            Permissions = [];
        }

        public string? User { get; set; }
        public List<string?> Permissions { get; set; }

        public bool HasPermission(string permission)
        {
            return Permissions.Contains(permission);
        }
    }
}
