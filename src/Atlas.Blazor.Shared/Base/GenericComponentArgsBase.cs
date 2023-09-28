using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Base
{
    public class GenericComponentArgsBase : ComponentArgsBase
    {
        [Inject]
        public IGenericRequests? GenericRequests { get; set; }
    }
}
