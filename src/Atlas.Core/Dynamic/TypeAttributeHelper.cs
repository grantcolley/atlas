using Atlas.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atlas.Core.Dynamic
{
    public class TypeAttributeHelper
    {
        public static IEnumerable<AtlasType> GetHeadwayTypesByAttribute(Type attributeType)
        {
            var dynamicTypes = new List<AtlasType>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .ToList();

            foreach (var assembly in assemblies)
            {
                var types = (from t in assembly.GetTypes()
                             let attributes = t.GetCustomAttributes(attributeType, true)
                             where attributes != null && attributes.Length > 0
                             select new AtlasType
                             {
                                 Name = GetName(t),
                                 DisplayName = GetName(t),
                                 Namespace = GetFullNamespace(t, assembly)
                             }).ToList();

                dynamicTypes.AddRange(types);
            }

            return dynamicTypes;
        }

        private static string GetName(Type type)
        {
            return (type.IsAbstract && type.Name.Contains(TypeHelpers.BASE))
                ? type.Name.Replace(TypeHelpers.BASE, string.Empty)
                : type.Name;
        }

        private static string GetFullNamespace(Type type, Assembly assembly)
        {
            return (type.IsAbstract && type.Name.Contains(TypeHelpers.BASE))
                ? $"{type.Namespace}.{type.Name.Replace(TypeHelpers.BASE, string.Empty)}, {assembly.GetName().Name}"
                : $"{type.FullName}, {assembly.GetName().Name}";
        }
    }
}
