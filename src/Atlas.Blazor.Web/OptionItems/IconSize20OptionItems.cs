using Atlas.Core.Interfaces;
using Atlas.Core.Models;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Reflection;

namespace Atlas.Blazor.Web.OptionItems
{
    public class IconSize20OptionItems : IOptionItems
    {
        private static IEnumerable<OptionItem>? _iconsSize20;

        public IEnumerable<OptionItem> GetOptionItems(IEnumerable<OptionsArg> args)
        {
            if (_iconsSize20 != null) return _iconsSize20;

            Assembly? assembly = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .FirstOrDefault(i => i.ManifestModule.Name == "Microsoft.FluentUI.AspNetCore.Components.Icons.dll");

            if (assembly == null) throw new InvalidOperationException(nameof(assembly));

            _iconsSize20 = [.. assembly.GetTypes()
                .Where(i => i.BaseType == typeof(Icon) && !string.IsNullOrWhiteSpace(i.FullName) && i.FullName.Contains("Regular+Size20"))
                .Select(i => new OptionItem { Id = i.Name, Display = i.Name, Icon = i.Name })
                .OrderBy(oi => oi.Display)];

            return _iconsSize20;
        }
    }
}
