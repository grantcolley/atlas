using Atlas.Blazor.Core.Constants;
using Atlas.Blazor.Core.Interfaces;
using Atlas.Blazor.Core.Models;
using Atlas.Core.Authentication;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Atlas.Blazor.Core.Base
{
    public abstract class AtlasComponentBase : ComponentBase, IDisposable
    {
        [Inject]
        public PersistentComponentState? ApplicationState { get; set; }

        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IStateNotificationService? StateNotificationService { get; set; }

        [Inject]
        public TokenProvider? TokenProvider { get; set; }

        //[Inject]
        //public IDialogService? DialogService { get; set; }

        public TokenProvider? LocalTokenProvider { get; set; }

        private PersistingComponentStateSubscription persistingComponentStateSubscription;

        protected override async Task OnInitializedAsync()
        {
            if (ApplicationState == null) throw new NullReferenceException(nameof(ApplicationState));

            Debug.WriteLine($"### {GetType().Name} - AtlasComponentBase - OnInitializedAsync - START {TokenProvider?.AccessToken?.Substring(0, 10)}");

            if (TokenProvider != null)
            {
                if (ApplicationState.TryTakeFromJson<TokenProvider>($"{GetType().Name}-{PersistState.TOKEN_PROVIDER}", out TokenProvider? tokenProvider))
                {
                    Debug.WriteLine($"### {GetType().Name} - AtlasComponentBase - OnInitializedAsync - TryTakeFromJson - {GetType().Name}-{PersistState.TOKEN_PROVIDER} - TRUE - {tokenProvider?.AccessToken?.Substring(0, 10)}");
                    LocalTokenProvider = tokenProvider;
                }
                else
                {
                    persistingComponentStateSubscription = ApplicationState.RegisterOnPersisting(PersistTokenProvider);

                    Debug.WriteLine($"### {GetType().Name} - AtlasComponentBase - OnInitializedAsync - TryTakeFromJson - {GetType().Name}-{PersistState.TOKEN_PROVIDER} - FALSE - {TokenProvider?.AccessToken?.Substring(0, 10)}");
                    LocalTokenProvider = TokenProvider;
                }
            }
            
            Debug.WriteLine($"### {GetType().Name} - AtlasComponentBase - OnInitializedAsync - END");
        }

        protected void SetBearerToken(IRequestBase? requestBase)
        {
            if (requestBase == null) throw new NullReferenceException(nameof(requestBase));
            
            requestBase.SetBearerToken(LocalTokenProvider);
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

            await StateNotificationService.NotifyStateHasChangedAsync(StateNotifications.BREADCRUMBS, breadcrumb)
                .ConfigureAwait(false);
        }

        private Task PersistTokenProvider()
        {
            if (ApplicationState == null) throw new NullReferenceException(nameof(ApplicationState));
            if (TokenProvider == null) throw new NullReferenceException(nameof(TokenProvider));

            Debug.WriteLine($"### {GetType().Name} - AtlasComponentBase - PersistTokenProvider - START");

            if (!string.IsNullOrWhiteSpace(TokenProvider.AccessToken))
            {
                Debug.WriteLine($"### {GetType().Name} - AtlasComponentBase - PersistTokenProvider - PersistAsJson - {GetType().Name}-{PersistState.TOKEN_PROVIDER} - {TokenProvider?.AccessToken?.Substring(0, 10)}");
                ApplicationState.PersistAsJson($"{GetType().Name}-{PersistState.TOKEN_PROVIDER}", TokenProvider);
            }

            Debug.WriteLine($"### {GetType().Name} - AtlasComponentBase - PersistTokenProvider - END");

            return Task.CompletedTask;
        }

        void IDisposable.Dispose()
        {
            persistingComponentStateSubscription.Dispose();

            Debug.WriteLine($"### {GetType().Name} - AtlasComponentBase - Dispose");
        }
    }
}
