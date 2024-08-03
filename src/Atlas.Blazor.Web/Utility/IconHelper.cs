using Atlas.Blazor.Web.Constants;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Atlas.Blazor.Web.Utility
{
    public static class IconHelper
    {
        public static Icon GetRegularSize20(string? name)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                name = AtlasWebConstants.ATLAS_DEFAULT_ICON;
            }

            try
            {
                IconInfo iconInfo = new() { Name = name, Size = IconSize.Size20, Variant = IconVariant.Regular };

                return Icons.GetInstance(iconInfo);
            }
            catch (ArgumentException)
            {
                return new Icons.Regular.Size20.Prohibited();
            }
        }
    }
}
