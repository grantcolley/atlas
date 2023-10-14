using Atlas.Blazor.Shared.Base;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Interfaces;

namespace Atlas.Blazor.Shared.Components.Admin
{
    public abstract class ModulesViewBase : GenericPageArgsBase
    {
        protected IEnumerable<Module>? _modules;
        protected bool _loaded = false;

        protected override async Task OnInitializedAsync()
        {
            if (GenericRequests == null) throw new ArgumentNullException(nameof(GenericRequests));

            await base.OnInitializedAsync().ConfigureAwait(false);

            IResponse<IEnumerable<Module>> response = await GenericRequests.GetListAsync<Module>(AtlasAPIEndpoints.GET_MODULES)
                .ConfigureAwait(false);

            if(response.IsSuccess)
            {
                _modules = response.Result;
            }
            else if(!string.IsNullOrWhiteSpace(response.Message))
            {
                RaiseAlert(response.Message);
            }

            _loaded = true;
        }
    }
}
