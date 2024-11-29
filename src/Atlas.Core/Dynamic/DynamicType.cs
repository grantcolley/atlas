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
    /// <remarks>
    /// Initialises a new instance of the DynamicTypeHelper.
    /// </remarks>
    /// <param name="createInstance">A dynamic method for creating new instance of the specified type.</param>
    /// <param name="getters">A dictionary of dynamic methods for property getters.</param>
    /// <param name="setters">A dictionary of dynamic methods for property setters.</param>
    /// <param name="supportedProperties">A list of property info's for supported properties.</param>
    public class DynamicType<T>(Func<T> createInstance,
        Dictionary<string, Func<T, object?>> getters,
        Dictionary<string, Action<T, object?>> setters,
        IEnumerable<PropertyInfo> supportedProperties)
    {
        private readonly Dictionary<string, Func<T, object?>> _getters = getters;
        private readonly Dictionary<string, Action<T, object?>> _setters = setters;

        /// <summary>
        /// Gets the declaring type.
        /// </summary>
        public Type DeclaringType { get; private set; } = typeof(T);

        /// <summary>
        /// Gets a dynamic method for creating new instance of the specified type
        /// </summary>
        public Func<T> CreateInstance { get; } = createInstance;

        /// <summary>
        /// Gets a list of property info's for supported properties.
        /// </summary>
        public IEnumerable<PropertyInfo> SupportedProperties { get; } = supportedProperties;

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
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1854 // Prefer the 'IDictionary.TryGetValue(TKey, out TValue)' method
            if (!_setters.ContainsKey(fieldName))
            {
                throw new KeyNotFoundException(fieldName + " not supported.");
            }
#pragma warning restore CA1854 // Prefer the 'IDictionary.TryGetValue(TKey, out TValue)' method
#pragma warning restore IDE0079 // Remove unnecessary suppression

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
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1854 // Prefer the 'IDictionary.TryGetValue(TKey, out TValue)' method
            if (!_getters.ContainsKey(fieldName))
            {
                throw new KeyNotFoundException(fieldName + " not supported.");
            }
#pragma warning restore CA1854 // Prefer the 'IDictionary.TryGetValue(TKey, out TValue)' method
#pragma warning restore IDE0079 // Remove unnecessary suppression

            return _getters[fieldName](target);
        }
    }
}
