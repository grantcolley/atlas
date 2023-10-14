using Atlas.Blazor.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Base
{
    public class PageArgsBase : ComponentBase
    {
        [Parameter]
        public PageArgs? PageArgs { get; set; }
    }
}
