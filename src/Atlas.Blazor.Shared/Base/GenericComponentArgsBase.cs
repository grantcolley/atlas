using Atlas.Blazor.Shared.Models;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Base
{
    public class GenericComponentArgsBase : AtlasComponentBase
    {
        [Inject]
        public IGenericRequests? GenericRequests { get; set; }

        [Parameter]
        public PageArgs? PageArgs { get; set; }
    }
}
