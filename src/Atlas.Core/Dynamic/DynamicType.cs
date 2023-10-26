using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atlas.Core.Dynamic
{
    /// <summary>
    /// A dynamic type helper containing dynamic methods for the specified type.
    /// </summary>
    /// <typeparam name="T">The specified type.</typeparam>
    public class DynamicType<T>
    {
        private readonly Dictionary<string, Func<T, object?>> _getters;
        private readonly Dictionary<string, Action<T, object?>> _setters;

        /// <summary>
        /// Initialises a new instance of the DynamicTypeHelper.
        /// </summary>
        /// <param name="createInstance">A dynamic method for creating new instance of the specified type.</param>
        /// <param name="getters">A dictionary of dynamic methods for property getters.</param>
        /// <param name="setters">A dictionary of dynamic methods for property setters.</param>
        /// <param name="supportedProperties">A list of property info's for supported properties.</param>
        public DynamicType(Func<T> createInstance,
            Dictionary<string, Func<T, object?>> getters,
            Dictionary<string, Action<T, object?>> setters,
            IEnumerable<PropertyInfo> supportedProperties)
        {
            DeclaringType = typeof(T);

            _getters = getters;
            _setters = setters;
            CreateInstance = createInstance;
            SupportedProperties = supportedProperties;
        }

        /// <summary>
        /// Gets the declaring type.
        /// </summary>
        public Type DeclaringType { get; private set; }

        /// <summary>
        /// Gets a dynamic method for creating new instance of the specified type
        /// </summary>
        public Func<T> CreateInstance { get; }

        /// <summary>
        /// Gets a list of property info's for supported properties.
        /// </summary>
        public IEnumerable<PropertyInfo> SupportedProperties { get; }

        /// <summary>
        /// Gets a <see cref="PropertyInfo"/> from the <see cref="SupportedProperties"/> list.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The <see cref="PropertyInfo"/> matching the propertyName</returns>
        public PropertyInfo GetPropertyInfo(string propertyName)
        {
            return SupportedProperties.First(p => p.Name == propertyName);
        }

        /// <summary>
        /// A dynamic method for setting the value of the target property.
        /// </summary>
        /// <param name="target">The target property.</param>
        /// <param name="fieldName">The property name.</param>
        /// <param name="value">The value to set.</param>
        public void SetValue(T target, string fieldName, object? value)
        {
            if (!_setters.ContainsKey(fieldName))
            {
                throw new KeyNotFoundException(fieldName + " not supported.");
            }

            _setters[fieldName](target, value);
            return;
        }

        /// <summary>
        /// A dynamic method for getting the value of the target property.
        /// </summary>
        /// <param name="target">The target property.</param>
        /// <param name="fieldName">The property name.</param>
        /// <returns>The value of the property.</returns>
        public object? GetValue(T target, string fieldName)
        {
            if (!_getters.ContainsKey(fieldName))
            {
                throw new KeyNotFoundException(fieldName + " not supported.");
            }

            return _getters[fieldName](target);
        }
    }
}
