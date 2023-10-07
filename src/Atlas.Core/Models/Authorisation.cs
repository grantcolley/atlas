using System;
using System.Collections.Generic;

namespace Atlas.Core.Models
{
    public class Authorisation
    {
        public Authorisation()
        {
            Permissions = new List<string?>();
        }

        public string? User { get; set; }
        public string? Theme { get; set; }
        public List<string?> Permissions { get; set; }

        public bool HasPermission(string permission)
        {
            if(Permissions == null) 
            {
                throw new ArgumentNullException(nameof(permission));
            }

            return Permissions.Contains(permission);
        }
    }
}
