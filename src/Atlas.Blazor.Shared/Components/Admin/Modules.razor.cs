using Atlas.Blazor.Shared.Base;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Interfaces;

namespace Atlas.Blazor.Shared.Components.Admin
{
    public abstract class ModulesBase : AtlasGenericComponentBase
    {
        protected IEnumerable<Module>? _modules;
        protected IEnumerable<string>? _fields;
        protected string? _navigationEndpoint;
        protected string? _identifierField;

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

            Dictionary<string, string> parameters = ComponentArgs.GetComponentParameters();

            string? fieldsDelimiter = parameters["FieldsDelimiter"];

            _fields = parameters["Fields"].Split(fieldsDelimiter);

            _identifierField = parameters["IdentifierField"];

            _navigationEndpoint = AtlasAPIEndpoints.GET_MODULE;

            IResponse<IEnumerable<Module>> response = await GenericRequests.GetGenericListAsync<Module>(AtlasAPIEndpoints.GET_MODULES)
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
