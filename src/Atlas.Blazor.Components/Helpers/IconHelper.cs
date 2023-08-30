namespace Atlas.Blazor.Components.Helpers
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
    }
}
