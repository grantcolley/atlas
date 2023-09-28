using Atlas.Blazor.Shared.Base;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Interfaces;

namespace Atlas.Blazor.Shared.Components.Admin
{
    public abstract class ModuleViewBase : GenericComponentArgsBase
    {
        protected string _identityField = "ModuleId";
        protected string _titleField = "Name";
        protected string _createEndpoint = AtlasAPIEndpoints.CREATE_MODULE;
        protected string _updateEndpoint = AtlasAPIEndpoints.UPDATE_MODULE;
        protected string _deleteEndpoint = AtlasAPIEndpoints.DELETE_MODULE;
        protected bool _loaded = false;
        protected Module? _module = default;

        protected override async Task OnInitializedAsync()
        {
            if (GenericRequests == null)
            {
                throw new ArgumentNullException(nameof(GenericRequests));
            }

            if (ComponentArgs == null)
            {
                throw new ArgumentNullException(nameof(ComponentArgs));
            }

            await base.OnInitializedAsync().ConfigureAwait(false);

            IResponse<Module> response = await GenericRequests.GetModelAsync<Module>(ComponentArgs.ModelInstanceId, AtlasAPIEndpoints.GET_MODULE)
                .ConfigureAwait(false);

            if (response.IsSuccess)
            {
                _module = response.Result;
            }
            else if (!string.IsNullOrWhiteSpace(response.Message))
            {
                RaiseAlert(response.Message);
            }

            _loaded = true;
        }
    }
}
