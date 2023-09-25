using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlas.Core.Models
{
    public class ComponentArgs : ModelBase
    {
        public int ComponentArgsId { get; set; }
        public bool NavigateResetBreadcrumb { get; set; }

        [Required]
        [StringLength(30)]
        public string ComponentCode { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string ComponentName { get; set; } = string.Empty;

        [StringLength(150)]
        public string? ComponentParameters { get; set; } = string.Empty;

        [StringLength(50)]
        public string? RoutingPage { get; set; } = string.Empty;

        [StringLength(30)]
        public string? RoutingComponentCode { get; set; } = string.Empty;

        [NotMapped]
        public int ModelInstanceId { get; set; }

        public Type? GetComponentType()
        {
            if(string.IsNullOrWhiteSpace(ComponentName))
            {
                return null;
            }

            return Type.GetType(ComponentName);
        }

        public Dictionary<string, object> ToDynamicComponentParameters()
        {
            return new()
            {
                { "ComponentArgs", this }
            };
        }

        public Dictionary<string, string> GetComponentParameters()
        {
            Dictionary<string, string> parameters = new();

            if(!string.IsNullOrWhiteSpace(ComponentParameters))
            {
                string[] elements = ComponentParameters.Split(';');

                if(elements.Length > 0) 
                {
                    foreach(string keyValue in elements) 
                    {
                        if(!string.IsNullOrWhiteSpace(keyValue))
                        {
                            string[] pair = keyValue.Split("=");

                            if(pair.Length == 2)
                            {
                                parameters[pair[0]] = pair[1];
                            }
                        }
                    }
                }
            }

            return parameters;
        }
    }
}
