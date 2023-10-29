using Atlas.Blazor.UI.Base;
using Atlas.Blazor.UI.Models;
using Atlas.Core.Dynamic;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.UI.Components.Generic
{
    public abstract class GenericModelsViewBase<T> : GenericPageArgsBase where T : class, new()
    {
        [Parameter]
        public IEnumerable<T> Source { get; set; } = Enumerable.Empty<T>();

        protected bool FilterFunction(T item) => FilterItem(item, FilterString);

        protected IEnumerable<string> Fields = Enumerable.Empty<string>();
        
        protected DynamicType<T>? DynamicType;

        protected string? FilterString;

        private string _routingPath = string.Empty;

        private string _identifierField = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            if(PageArgs == null) throw new ArgumentNullException(nameof(PageArgs));

            Fields = (IEnumerable<string>)PageArgs.ModelParameters["Fields"];

            _identifierField = (string)PageArgs.ModelParameters["IdentifierField"];

            _routingPath = $@"{PageArgs.RoutingPage}\{PageArgs.RoutingPageCode}";

            await base.OnInitializedAsync().ConfigureAwait(false);

            DynamicType = DynamicTypeHelper.Get<T>();

            await SendBreadcrumbAsync(BreadcrumbAction.Add, PageArgs.DisplayName)
                .ConfigureAwait(false);
        }

        protected void HeaderButtonClick()
        {
            if (string.IsNullOrEmpty(_identifierField))
            {
                NavigationManager?.NavigateTo(_routingPath);
            }
            else
            {
                NavigationManager?.NavigateTo($"{_routingPath}/{0}");
            }
        }

        protected void RowButtonClick(T item)
        {
            if(string.IsNullOrEmpty(_identifierField))
            {
                NavigationManager?.NavigateTo(_routingPath);
            }
            else
            {
                object? id = DynamicType?.GetValue(item, _identifierField);
                NavigationManager?.NavigateTo($"{_routingPath}/{id}");
            }
        }

        private bool FilterItem(T item, string? filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            foreach (var field in Fields)
            {
                string? value = DynamicType?.GetValue(item, field)?.ToString();

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
