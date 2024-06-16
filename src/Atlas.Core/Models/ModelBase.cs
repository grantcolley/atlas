using System;

namespace Atlas.Core.Models
{
    public abstract class ModelBase
    {
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
