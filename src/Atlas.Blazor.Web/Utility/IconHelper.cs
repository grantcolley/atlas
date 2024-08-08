using Atlas.Blazor.Web.Constants;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Atlas.Blazor.Web.Utility
{
    public static class IconHelper
    {
        public static Color GetIconColor(string? colour)
        {
            if (string.IsNullOrWhiteSpace(colour)) return Color.Accent;

            return colour switch
            {
                "Error" => Color.Error,
                "Warning" => Color.Warning,
                "Information" => Color.Success,
                _ => Color.Accent,
            };
        }

        public static Icon GetRegularSize24(string? name)
        {
            return GetRegularIcon(name, IconSize.Size24);
        }

        public static Icon GetRegularSize20(string? name)
        {
            return GetRegularIcon(name, IconSize.Size20);
        }

        public static Icon GetRegularSize16(string? name)
        {
            return GetRegularIcon(name, IconSize.Size16);
        }

        private static Icon GetRegularIcon(string? name, IconSize iconSize = IconSize.Size20)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                name = AtlasWebConstants.ATLAS_DEFAULT_ICON;
            }

            try
            {
                IconInfo iconInfo = new() { Name = name, Size = iconSize, Variant = IconVariant.Regular };

                return Icons.GetInstance(iconInfo);
            }
            catch (ArgumentException)
            {
                return GetRegularIcon("Prohibited", iconSize);
            }
        }
    }
}
