using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.Data.Context
{
    public class Audit
    {
        public int Id { get; set; }
        public string? ClrType { get; set; }
        public string? TableName { get; set; }
        public string? Action { get; set; }
        public string? EntityId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? User { get; set; }
        public DateTime DateTime { get; set; }

        [NotMapped]
        public List<PropertyEntry> TemporaryProperties { get; } = [];

        [NotMapped]
        public Dictionary<string, object?> OldValuesDictionary { get; } = [];

        [NotMapped]
        public Dictionary<string, object?> NewValuesDictionary { get; } = [];
    }
}
