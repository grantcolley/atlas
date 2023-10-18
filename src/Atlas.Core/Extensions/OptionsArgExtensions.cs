using Atlas.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Atlas.Core.Extensions
{
    public static class OptionsArgExtensions
    {
        public static OptionsArg? FirstOptionsArgDefault(this IEnumerable<OptionsArg> args, string name)
        {
            return args.FirstOrDefault(a => !string.IsNullOrWhiteSpace(a.Name) && a.Name.Equals(name));
        }

        public static string? FirstOptionsArgValue(this IEnumerable<OptionsArg> args, string name)
        {
            return args.First(a => !string.IsNullOrWhiteSpace(a.Name) && a.Name.Equals(name))?.Value;
        }

        public static bool HasOptionsArg(this IEnumerable<OptionsArg> args, string name)
        {
            return args.Any(a => !string.IsNullOrWhiteSpace(a.Name) && a.Name.Equals(name));
        }
    }
}
