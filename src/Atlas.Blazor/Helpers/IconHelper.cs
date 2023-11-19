using Atlas.Blazor.Constants;
using Atlas.Blazor.Models;
using MudBlazor;
using System.Reflection;

namespace Atlas.Blazor.Helpers
{
    public static class IconHelper
    {
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

        public static IEnumerable<IconOptionItem> GetIconOptionItems(string iconsCode)
        {
            if (iconsCode == null) throw new ArgumentNullException(nameof(iconsCode));

            if (iconsCode.Equals(IconsOptions.ALL))
            {
                return GetIconOptionItems();
            }
            else if(iconsCode.Equals(IconsOptions.THEME))
            {
                return GetThemeIconOptionItems();
            }
            else
            {
                throw new NotSupportedException(iconsCode);
            }
        }

        private static IEnumerable<IconOptionItem> GetIconOptionItems()
        {
            List<IconOptionItem> icons = new();

            FieldInfo[] fieldInfos = typeof(MudBlazor.Icons.Material.Outlined).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                icons.Add(new IconOptionItem { Id = fieldInfo.Name, Display = fieldInfo.Name, Icon = fieldInfo.GetRawConstantValue()?.ToString() });
            }

            return icons.OrderBy(i => i.Display);
        }

        private static IEnumerable<IconOptionItem> GetThemeIconOptionItems()
        {
            return new List<IconOptionItem>
            {
                new IconOptionItem { Id = "DarkMode", Display = "Dark Mode", Icon = @Icons.Material.Filled.DarkMode },
                new IconOptionItem { Id = "LightMode", Display = "Light Mode", Icon = @Icons.Material.Filled.LightMode }
            };
        }
    }
}
