﻿using Atlas.Blazor.Web.Constants;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Web.Components.Base
{
    public abstract class AtlasComponentBase : ComponentBase
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        protected T? GetResponse<T>(IResponse<T> response)
        {
            if (!response.IsSuccess
                && !string.IsNullOrWhiteSpace(response.Message))
            {
                RouteAlert(response.Message);
                return default;
            }
            else
            {
                return response.Result;
            }
        }

        protected void RouteAlert(string message)
        {
            Models.Alert alert = new()
            {
                AlertType = Alerts.ERROR,
                Title = "Error",
                Message = message
            };

            NavigationManager?.NavigateTo(alert.Route);
        }
    }
}
