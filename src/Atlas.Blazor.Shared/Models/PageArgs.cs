namespace Atlas.Blazor.Shared.Models
{
    public class PageArgs
    {
        public string PageCode { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ComponentName { get; set; } = string.Empty;
        public string? ComponentParameters { get; set; } = string.Empty;
        public string? RoutingPage { get; set; } = string.Empty;
        public string? RoutingComponentCode { get; set; } = string.Empty;
        public int ModelInstanceId { get; private set; }

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
