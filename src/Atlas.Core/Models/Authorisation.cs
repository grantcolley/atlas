using System.Collections.Generic;

namespace Atlas.Core.Models
{
    public class Authorisation
    {
        public Authorisation()
        {
            Permissions = new List<string>();
        }

        public string? User { get; set; }
        public List<string> Permissions { get; set; }
    }
}
