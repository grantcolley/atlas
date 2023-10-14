namespace Atlas.Blazor.Shared.Models
{
    public class PageArgs
    {
        public string PageCode { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ComponentName { get; set; } = string.Empty;
        public string? RoutingPage { get; set; } = string.Empty;
        public string? RoutingPageCode { get; set; } = string.Empty;
        public int ModelInstanceId { get; private set; }
        public Dictionary<string, object> ModelParameters { get; set; } = new (); 

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
