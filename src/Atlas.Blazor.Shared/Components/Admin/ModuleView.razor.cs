using Atlas.Blazor.Shared.Base;
using Atlas.Core.Constants;

namespace Atlas.Blazor.Shared.Components.Admin
{
    public abstract class ModuleViewBase : ComponentArgsBase
    {
        protected string _identityField = "ModuleId";
        protected string _titleField = "Name";
        protected string _createEndpoint = AtlasAPIEndpoints.CREATE_MODULE;
        protected string _updateEndpoint = AtlasAPIEndpoints.UPDATE_MODULE;
        protected string _deleteEndpoint = AtlasAPIEndpoints.DELETE_MODULE;

        protected override void OnInitialized()
        {
            if (ComponentArgs == null)
            {
                throw new ArgumentNullException(nameof(ComponentArgs));
            }

            base.OnInitialized();
        }
    }
}
