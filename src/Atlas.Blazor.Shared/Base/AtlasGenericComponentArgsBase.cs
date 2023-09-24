using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Base
{
    public class AtlasGenericComponentArgsBase : AtlasComponentArgsBase
    {
        [Inject]
        public IGenericRequests? GenericRequests { get; set; }
    }
}
