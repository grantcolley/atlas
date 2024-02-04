using Atlas.Core.Authentication;
using System;
using System.Collections.Generic;

namespace Atlas.Blazor.Web.App.Models
{
    public class PageArgs
    {
        public string PageCode { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ComponentName { get; set; } = string.Empty;
        public string? RoutingPage { get; set; } = string.Empty;
        public string? RoutingPageCode { get; set; } = string.Empty;
        public int ModelInstanceId { get; private set; }
        public TokenProvider? TokenProvider { get; set; }

        public Dictionary<string, string> ModelParameters { get; set; } = new (); 

        public void SetModelInstanceId(int id)
        {
            ModelInstanceId = id;
        }

        public Type? GetComponentType()
        {
            if(string.IsNullOrWhiteSpace(ComponentName))
            {
                return null;
            }

            return Type.GetType(ComponentName);
        }

        public Dictionary<string, object> ToParametersDictionary()
        {
            return new()
            {
                { GetType().Name, this }
            };
        }
    }
}
