using Atlas.Core.Models;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Base
{
    public class AtlasComponentArgsBase : AtlasComponentBase
    {
        [Parameter]
        public ComponentArgs? ComponentArgs { get; set; }
    }
}
