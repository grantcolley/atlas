﻿using Atlas.Core.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Components.Generic
{
    public abstract class TableViewBase<T> : ComponentBase where T : class, new()
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Parameter]
        public string DisplayHeader { get; set; } = string.Empty;

        [Parameter]
        public string NavigationEndpoint { get; set; } = string.Empty;

        [Parameter]
        public IEnumerable<T> Source { get; set; } = Enumerable.Empty<T>();

        [Parameter]
        public IEnumerable<string> Fields { get; set; } = Enumerable.Empty<string>();

        [Parameter]
        public string IdentifierField { get; set; } = string.Empty;
        
        protected DynamicType<T>? DynamicType;

        protected string? FilterString;

        protected bool FilterFunction(T item) => FilterItem(item, FilterString);

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(false);

            DynamicType = DynamicTypeHelper.Get<T>();
        }

        protected void HeaderButtonClick()
        {
            NavigationManager?.NavigateTo($"{NavigationEndpoint}/{0}");
        }

        protected void RowButtonClick(T item)
        {
            object? id = DynamicType?.GetValue(item, IdentifierField);
            NavigationManager?.NavigateTo($"{NavigationEndpoint}/{id}");
        }

        private bool FilterItem(T item, string? filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            foreach (var field in Fields)
            {
                string? value = DynamicType?.GetValue(item, field).ToString();

                if (value != null
                    && value.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}