using Atlas.Core.Models;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Web.Components.Generic
{
    public abstract class GridContainerBase<T> : GenericGridContainer<T> where T : ModelBase, new()
    {
        public abstract string? Title { get; set; }
        public abstract string? NavigateTo { get; set; }
        public abstract string? APIEndpoint { get; set; }
        public abstract int ItemsPerPage { get; set; }
        public abstract string? FilterFieldName { get; set; }
        public abstract string? IdentityFieldName { get; set; }
        public abstract string? CreatePermission { get; set; }

        public abstract RenderFragment RenderGridContent();

        protected override async Task OnInitializedAsync()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(Title);
            ArgumentException.ThrowIfNullOrWhiteSpace(NavigateTo);
            ArgumentException.ThrowIfNullOrWhiteSpace(APIEndpoint);
            ArgumentException.ThrowIfNullOrWhiteSpace(FilterFieldName);
            ArgumentException.ThrowIfNullOrWhiteSpace(IdentityFieldName);

            _title = Title;
            _navigateTo = NavigateTo;
            _apiEndpoint = APIEndpoint;
            _itemsPerPage = ItemsPerPage;
            _filterFieldName = FilterFieldName;
            _identityFieldName = IdentityFieldName;
            _createPermission = CreatePermission;

            await base.OnInitializedAsync();
        }

        public override RenderFragment RenderGrid()
        {
            return RenderGridContent();
        }
    }
}
