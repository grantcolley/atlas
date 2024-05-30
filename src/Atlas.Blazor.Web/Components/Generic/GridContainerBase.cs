﻿using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Web.Components.Generic
{
    public abstract class GridContainerBase<T> : GenericGridContainer<T> where T : class, new()
    {
        public abstract string? Title { get; set; }
        public abstract string? NavigateTo { get; set; }
        public abstract string? APIEndpoint { get; set; }
        public abstract int ItemsPerPage { get; set; }
        public abstract string? FilterFieldName { get; set; }
        public abstract string? IdentityFieldName { get; set; }

        public abstract RenderFragment RenderGrid();

        protected override async Task OnInitializedAsync()
        {
            System.Diagnostics.Debug.WriteLine($"{GetType().Name} abstract - OnInitialized - START");

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

            await base.OnInitializedAsync();

            System.Diagnostics.Debug.WriteLine($"{GetType().Name} abstract - OnInitialized - END");
        }

        public override RenderFragment RenderGridBase()
        {
            return RenderGrid();
        }
    }
}
