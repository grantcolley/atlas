using Atlas.Blazor.Web.App.Base;
using Atlas.Blazor.Web.App.Models;
using Atlas.Core.Dynamic;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Atlas.Blazor.Web.App.Components.Generic
{
    public abstract class GenericModelsViewBase<T> : GenericPageArgsBase where T : class, new()
    {
        [Parameter]
        public string? Endpoint { get; set; }

        protected IQueryable<T>? Source = Enumerable.Empty<T>().AsQueryable();

        protected bool FilterFunction(T item) => FilterItem(item, FilterString);

        protected IEnumerable<string> Fields = Enumerable.Empty<string>();
        
        protected DynamicType<T> DynamicType = DynamicTypeHelper.Get<T>();

        protected string? FilterString;

        protected bool Loaded = false;

        private string _routingPath = string.Empty;

        private string _identifierField = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            Debug.WriteLine($"### {GetType().Name} - GenericModelsViewBase<T> - OnInitializedAsync - START");

            if (PageArgs == null) throw new NullReferenceException(nameof(PageArgs));
            if (GenericRequests == null) throw new NullReferenceException(nameof(GenericRequests));
            //if (PageRouter == null) throw new NullReferenceException(nameof(PageRouter));
            if (string.IsNullOrWhiteSpace(Endpoint)) throw new NullReferenceException(nameof(Endpoint));

            await base.OnInitializedAsync().ConfigureAwait(false);

            //Debug.WriteLine($"### {GetType().Name} - GenericModelsViewBase<T> - OnInitializedAsync - PageRouter.GetAccessToken() {PageRouter.GetAccessToken()}");

            //if(PageRouter.DataLoaded)
            //{
            //    return;
            //}

            SetBearerToken(GenericRequests);

            IResponse<IEnumerable<T>> response = await GenericRequests.GetListAsync<T>(Endpoint)
                .ConfigureAwait(false);

            if (!response.IsSuccess)
            {
                Debug.WriteLine($"### {GetType().Name} - GenericModelsViewBase<T> - OnInitializedAsync - {Endpoint} {response.IsSuccess} {response.Message}");

                if (!string.IsNullOrWhiteSpace(response.Message))
                {
                    RaiseAlert(response.Message);
                }
            }
            else
            {
                Debug.WriteLine($"### {GetType().Name} - GenericModelsViewBase<T> - OnInitializedAsync - {Endpoint} {response.IsSuccess}");

                Source = response.Result?.AsQueryable();

                //PageRouter.DataLoaded = true;

                Fields = PageArgs.ModelParameters["Fields"].Split(";");

                _identifierField = (string)PageArgs.ModelParameters["IdentifierField"];

                _routingPath = $@"{PageArgs.RoutingPage}\{PageArgs.RoutingPageCode}";

                //await SendBreadcrumbAsync(BreadcrumbAction.Add, PageArgs.DisplayName)
                //    .ConfigureAwait(false);

                Loaded = true;
            }

            Debug.WriteLine($"### {GetType().Name} - GenericModelsViewBase<T> - OnInitializedAsync - END");
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
