using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Base
{
    public class AtlasGenericComponentBase : AtlasComponentBase
    {
        [Inject]
        public IGenericRequests? GenericRequests { get; set; }
    }
}
