using Atlas.Blazor.Constants;
using Atlas.Blazor.Models;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Base
{
    public class GenericPageArgsBase : AtlasComponentBase
    {
        [Inject]
        public IGenericRequests? GenericRequests { get; set; }

        [Parameter]
        public PageArgs? PageArgs { get; set; }
    }
}
