using Atlas.Blazor.UI.Models;
using System.Reflection;

namespace Atlas.Blazor.UI.Helpers
{
    public static class IconHelper
    {
        public static IEnumerable<IconOptionItem> GetIconOptionItems()
        { 
            List<IconOptionItem> icons = new();

            FieldInfo[] fieldInfos = typeof(MudBlazor.Icons.Material.Outlined).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (FieldInfo fieldInfo in fieldInfos) 
            {
                icons.Add(new IconOptionItem { Id = fieldInfo.Name, Display = fieldInfo.Name, Icon = fieldInfo.GetRawConstantValue()?.ToString() });
            }

            return icons.OrderBy(i => i.Display);
        }

        public static string? GetOutlined(string? name)
        {
            if (name == null)
            {
                return string.Empty;
            }

            var fieldInfo = typeof(MudBlazor.Icons.Material.Outlined).GetField(name);

            if (fieldInfo == null)
            {
                return string.Empty;
            }

            return fieldInfo.GetValue(null)?.ToString();
        }

        public static string? GetFilled(string? name)
        {
            if (name == null)
            {
                return string.Empty;
            }

            var fieldInfo = typeof(MudBlazor.Icons.Material.Filled).GetField(name);

            if (fieldInfo == null)
            {
                return string.Empty;
            }

            return fieldInfo.GetValue(null)?.ToString();
        }
    }
}
