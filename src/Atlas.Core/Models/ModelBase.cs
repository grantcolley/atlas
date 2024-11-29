using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.Core.Models
{
    public abstract class ModelBase
    {
        private bool _isReadOnly;

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

        [NotMapped]
        public bool IsReadOnly 
        {
            get { return _isReadOnly; }
            set
            {
                if(_isReadOnly != value)
                {
                    _isReadOnly = value;
                    PropagateReadOnly();
                }
            }
        }

        public virtual int? GetId() { return null; }

        /// <summary>
        /// Overriden by derived classes for propagating <see cref="IsReadOnly"/>
        /// to all it's properties that also inherit from <see cref="ModelBase"/>.
        /// </summary>
        protected virtual void PropagateReadOnly() { }
    }
}
