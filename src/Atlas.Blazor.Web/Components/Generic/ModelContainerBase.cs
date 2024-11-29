using Atlas.Core.Models;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Web.Components.Generic
{
    public abstract class ModelContainerBase<T> : GenericModelContainer<T> where T : ModelBase, new()
    {
        public abstract string? Title { get; set; }
        public abstract string? APIGetEndpoint { get; set; }
        public abstract string? APICreateEndpoint { get; set; }
        public abstract string? APIUpdateEndpoint { get; set; }
        public abstract string? APIDeleteEndpoint { get; set; }
        public abstract string? IdentityFieldName { get; set; }
        public abstract string? ModelNameField { get; set; }

        public abstract RenderFragment RenderModelContent();

        protected override async Task OnInitializedAsync()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(Title);
            ArgumentException.ThrowIfNullOrWhiteSpace(APIGetEndpoint);
            ArgumentException.ThrowIfNullOrWhiteSpace(IdentityFieldName);
            ArgumentException.ThrowIfNullOrWhiteSpace(ModelNameField);

            _title = Title;
            _apiGetEndpoint = APIGetEndpoint;
            _apiCreateEndpoint = APICreateEndpoint;
            _apiUpdateEndpoint = APIUpdateEndpoint;
            _apiDeleteEndpoint = APIDeleteEndpoint;
            _identityFieldName = IdentityFieldName;
            _modelNameField = ModelNameField;

            await base.OnInitializedAsync();
        }

        public override RenderFragment RenderModel()
        {
            return RenderModelContent();
        }
    }
}
