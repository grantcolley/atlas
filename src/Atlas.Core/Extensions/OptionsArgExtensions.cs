using Atlas.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Atlas.Core.Extensions
{
    public static class OptionsArgExtensions
    {
        public static string? FirstOptionsArgValue(this IEnumerable<OptionsArg> optionsArgs, string name)
        {
            return optionsArgs.First(a => !string.IsNullOrWhiteSpace(a.Name) && a.Name.Equals(name))?.Value;
        }
    }
}
