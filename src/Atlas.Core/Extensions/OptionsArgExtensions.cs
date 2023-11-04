using Atlas.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Atlas.Core.Extensions
{
    public static class OptionsArgExtensions
    {
        public static IEnumerable<OptionsArg> ToOptionsArgs(this Dictionary<string, string> parameters)
        { 
            List<OptionsArg> optionsArgs = new();

            foreach (KeyValuePair<string, string> kvp in parameters) 
            {
                optionsArgs.Add(new OptionsArg { Name = kvp.Key, Value = kvp.Value });
            }

            return optionsArgs;
        }

        public static OptionsArg? FirstOptionsArgDefault(this IEnumerable<OptionsArg> optionsArgs, string name)
        {
            return optionsArgs.FirstOrDefault(a => !string.IsNullOrWhiteSpace(a.Name) && a.Name.Equals(name));
        }

        public static string? FirstOptionsArgValue(this IEnumerable<OptionsArg> optionsArgs, string name)
        {
            return optionsArgs.First(a => !string.IsNullOrWhiteSpace(a.Name) && a.Name.Equals(name))?.Value;
        }

        public static bool HasOptionsArg(this IEnumerable<OptionsArg> optionsArgs, string name)
        {
            return optionsArgs.Any(a => !string.IsNullOrWhiteSpace(a.Name) && a.Name.Equals(name));
        }
    }
}
