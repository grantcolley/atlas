using Atlas.Blazor.Web.Constants;
using Atlas.Core.Interfaces;
using Atlas.Core.Models;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Reflection;

namespace Atlas.Blazor.Web.OptionItems
{
    public class IconSize20OptionItems : IOptionItems
    {
        public IEnumerable<OptionItem> GetOptionItems(IEnumerable<OptionsArg> args)
        {
            Assembly? assembly = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .FirstOrDefault(i => i.ManifestModule.Name == "Microsoft.FluentUI.AspNetCore.Components.Icons.dll");

            if (assembly == null) throw new InvalidOperationException(nameof(assembly));

            List<OptionItem> iconsSize20 = [.. assembly.GetTypes()
                .Where(i => i.BaseType == typeof(Icon) && i.FullName.Contains("Regular+Size20"))
                .Select(i => new OptionItem { Id = i.Name, Display = i.Name, Icon = i.Name })
                .OrderBy(oi => oi.Display)];

            return iconsSize20;
        }
    }
}
