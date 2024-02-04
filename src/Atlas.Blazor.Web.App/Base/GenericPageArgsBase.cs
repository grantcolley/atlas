using Atlas.Blazor.Web.App.Constants;
using Atlas.Blazor.Web.App.Interfaces;
using Atlas.Blazor.Web.App.Models;
using Atlas.Core.Authentication;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Atlas.Blazor.Web.App.Pages;
using Atlas.Core.Dynamic;

namespace Atlas.Blazor.Web.App.Base
{
    public class GenericPageArgsBase : ComponentBase, IDisposable
    {
        [Inject]
        public PersistentComponentState? ApplicationState { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IStateNotificationService? StateNotificationService { get; set; }

        [Inject]
        public IGenericRequests? GenericRequests { get; set; }

        [Parameter]
        public PageArgs? PageArgs { get; set; }

        protected TokenProvider? TokenProvider { get; set; }

        //[Inject]
        //public IDialogService? DialogService { get; set; }

        //private TokenProvider? _tokenProvider;

        private PersistingComponentStateSubscription persistingComponentStateSubscription;

        protected override async Task OnInitializedAsync()
        {
            if (PageArgs == null) throw new NullReferenceException(nameof(PageArgs));
            if (ApplicationState == null) throw new NullReferenceException(nameof(ApplicationState));

            Debug.WriteLine($"### {GetType().Name} - GenericPageArgsBase - OnInitializedAsync - START {TokenProvider?.AccessToken?.Substring(0, 10)}");

            await base.OnInitializedAsync();

            TokenProvider = PageArgs.TokenProvider;

            if (ApplicationState.TryTakeFromJson<TokenProvider>($"{GetType().Name}-{PersistState.TOKEN_PROVIDER}", out TokenProvider? tokenProvider))
            {
                Debug.WriteLine($"### {GetType().Name} - GenericPageArgsBase - ApplicationState.TryTakeFromJson TRUE - {tokenProvider?.AccessToken?.Substring(0, 10)}");
                TokenProvider = tokenProvider;
            }
            else
            {
                persistingComponentStateSubscription = ApplicationState.RegisterOnPersisting(PersistTokenProvider);

                Debug.WriteLine($"### {GetType().Name} - GenericPageArgsBase - ApplicationState.TryTakeFromJson FALSE - {TokenProvider?.AccessToken?.Substring(0, 10)}");
                TokenProvider = PageArgs.TokenProvider;
            }

            Debug.WriteLine($"### {GetType().Name} - GenericPageArgsBase - OnInitializedAsync - END");
        }

        protected void SetBearerToken(IRequestBase? requestBase)
        {
            if (requestBase == null) throw new NullReferenceException(nameof(requestBase));

            requestBase.SetBearerToken(TokenProvider);
        }

        protected T? GetResponse<T>(IResponse<T> response)
        {
            if (!response.IsSuccess
                && !string.IsNullOrWhiteSpace(response.Message))
            {
                RaiseAlert(response.Message);
                return default;
            }
            else
            {
                return response.Result;
            }
        }

        protected void RaiseAlert(string message)
        {
            var alert = new Models.Alert
            {
                AlertType = Alerts.ERROR,
                Title = "Error",
                Message = message
            };

            NavigationManager?.NavigateTo(alert.Page);
        }

        protected void RaiseAuthorisationAlert(string message)
        {
            var alert = new Models.Alert
            {
                AlertType = Alerts.ERROR,
                Title = "Access Denied",
                Message = message
            };

            NavigationManager?.NavigateTo(alert.Page);
        }

        protected async Task SendBreadcrumbAsync(BreadcrumbAction breadcrumbAction, string? text = null, string? href = null)
        {
            if (breadcrumbAction == BreadcrumbAction.Add
                && breadcrumbAction == BreadcrumbAction.Update
                && (string.IsNullOrWhiteSpace(text)
                || (string.IsNullOrWhiteSpace(href))))
            {
                return;
            }

            if (StateNotificationService == null) throw new NullReferenceException(nameof(StateNotificationService));
            if (NavigationManager == null) throw new NullReferenceException(nameof(NavigationManager));

            if (breadcrumbAction == BreadcrumbAction.Add)
            {
                href = NavigationManager.Uri.Remove(0, NavigationManager.BaseUri.Length - 1);
            }

            var breadcrumb = new Breadcrumb
            {
                Text = text,
                Href = href,
                BreadcrumbAction = breadcrumbAction
            };

            //await StateNotificationService.NotifyStateHasChangedAsync(StateNotifications.BREADCRUMBS, breadcrumb)
            //    .ConfigureAwait(false);
        }

        private Task PersistTokenProvider()
        {
            if (ApplicationState == null) throw new NullReferenceException(nameof(ApplicationState));
            if (TokenProvider == null) throw new NullReferenceException(nameof(TokenProvider));

            Debug.WriteLine($"### {GetType().Name} - GenericPageArgsBase - PersistTokenProvider - START");

            if (!string.IsNullOrWhiteSpace(TokenProvider.AccessToken))
            {
                Debug.WriteLine($"### {GetType().Name} - GenericPageArgsBase - PersistTokenProvider - PersistAsJson - {GetType().Name}-{PersistState.TOKEN_PROVIDER} - {TokenProvider?.AccessToken?.Substring(0, 10)}");
                ApplicationState.PersistAsJson($"{GetType().Name}-{PersistState.TOKEN_PROVIDER}", TokenProvider);
            }

            Debug.WriteLine($"### {GetType().Name} - GenericPageArgsBase - PersistTokenProvider - END");

            return Task.CompletedTask;
        }

        void IDisposable.Dispose()
        {
            persistingComponentStateSubscription.Dispose();

            Debug.WriteLine($"### {GetType().Name} - GenericComponentBase - Dispose");
        }
    }
}
