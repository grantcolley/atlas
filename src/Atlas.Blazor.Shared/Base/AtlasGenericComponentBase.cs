using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Base
{
    public class AtlasGenericComponentBase : AtlasComponentArgsBase
    {
        [Inject]
        public IGenericRequests? GenericRequests { get; set; }

        protected bool _loaded = false;
    }
}
