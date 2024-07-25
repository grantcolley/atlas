using System.Collections.Generic;

namespace Atlas.Core.Models
{
    public class ChecklistItem
    {
        public ChecklistItem()
        {
            SubItems = [];
        }

        public bool IsChecked { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<string?> SubItems { get; set; }
    }
}