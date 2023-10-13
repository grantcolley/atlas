using Atlas.Blazor.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Base
{
    public class ComponentArgsBase : ComponentBase
    {
        [Parameter]
        public PageArgs? ComponentArgs { get; set; }
    }
}
