using Microsoft.FluentUI.AspNetCore.Components;

namespace Atlas.Blazor.Web.Utility
{
    public static class IconHelper
    {
        public static Icon GetRegularSize20(string? name)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            IconInfo iconInfo = new() { Name = name, Size =  IconSize.Size20, Variant = IconVariant.Regular };
            return Icons.GetInstance(iconInfo);
        }
    }
}
