using System;

namespace Atlas.Core.Attributes
{
    /// <summary>
    /// Describes the component to render a property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ModelPropertyRenderAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the fully qualified name of the component 
        ///     e.g. 'Atlas.Core.MyClass, Atlas.Core'
        /// </summary>
        public string? Component { get; set; }

        /// <summary>
        /// Gets or sets the display label.
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets the order the property is displayed.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        public string? Tooltip { get; set; }

        /// <summary>
        /// Gets or sets the parameters associated with rendering the property in pipe / semi-colon name value pairs
        ///     e.g. 'Name=Parameter1;Value=Value1|Name=Parameter2;Value=Value2|Name=Parameter3;Value=Value3....'
        /// </summary>
        public string? Parameters { get; set; }
    }
}
