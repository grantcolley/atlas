//using Atlas.Blazor.Web.App.Constants;
//using Atlas.Blazor.Web.App.Interfaces;
//using Atlas.Blazor.Web.App.Models;
//using Atlas.Core.Authentication;
//using Atlas.Requests.Interfaces;
//using Microsoft.AspNetCore.Components;
//using System;
//using System.Threading.Tasks;

//namespace Atlas.Blazor.Web.App.Base
//{
//    public abstract class TokenProviderComponentBase : ComponentBase, IDisposable
//    {
//        [Inject]
//        public PersistentComponentState? ApplicationState { get; set; }

//        [Inject]
//        public NavigationManager? NavigationManager { get; set; }

//        [Inject]
//        public IStateNotificationService? StateNotificationService { get; set; }

//        [Inject]
//        public TokenProvider? TokenProvider { get; set; }

//        //[Inject]
//        //public IDialogService? DialogService { get; set; }

//        protected TokenProvider? _tokenProvider;

//        private PersistingComponentStateSubscription persistingComponentStateSubscription;

//        protected override async Task OnInitializedAsync()
//        {
//            await base.OnInitializedAsync();

//            RegisterPersistTokenProvider();
//        }

//        protected override void OnInitialized()
//        {
//            base.OnInitialized();

//            RegisterPersistTokenProvider();
//        }

//        protected void SetBearerToken(IRequestBase? requestBase)
//        {
//            if (requestBase == null) throw new NullReferenceException(nameof(requestBase));
            
//            requestBase.SetBearerToken(_tokenProvider);
//        }

//        protected T? GetResponse<T>(IResponse<T> response)
//        {
//            if (!response.IsSuccess
//                && !string.IsNullOrWhiteSpace(response.Message))
//            {
//                RaiseAlert(response.Message);
//                return default;
//            }
//            else
//            {
//                return response.Result;
//            }
//        }

//        protected void RaiseAlert(string message)
//        {
//            var alert = new Models.Alert
//            {
//                AlertType = Alerts.ERROR,
//                Title = "Error",
//                Message = message
//            };

//            NavigationManager?.NavigateTo(alert.Page);
//        }

//        protected void RaiseAuthorisationAlert(string message)
//        {
//            var alert = new Models.Alert
//            {
//                AlertType = Alerts.ERROR,
//                Title = "Access Denied",
//                Message = message
//            };

//            NavigationManager?.NavigateTo(alert.Page);
//        }

//        protected async Task SendBreadcrumbAsync(BreadcrumbAction breadcrumbAction, string? text = null, string? href = null)
//        {
//            if (breadcrumbAction == BreadcrumbAction.Add
//                && breadcrumbAction == BreadcrumbAction.Update
//                && (string.IsNullOrWhiteSpace(text)
//                || (string.IsNullOrWhiteSpace(href))))
//            {
//                return;
//            }

//            if (StateNotificationService == null) throw new NullReferenceException(nameof(StateNotificationService));
//            if (NavigationManager == null) throw new NullReferenceException(nameof(NavigationManager));

//            if (breadcrumbAction == BreadcrumbAction.Add)
//            {
//                href = NavigationManager.Uri.Remove(0, NavigationManager.BaseUri.Length - 1);
//            }

//            var breadcrumb = new Breadcrumb
//            {
//                Text = text,
//                Href = href,
//                BreadcrumbAction = breadcrumbAction
//            };

//            await StateNotificationService.NotifyStateHasChangedAsync(StateNotifications.BREADCRUMBS, breadcrumb)
//                .ConfigureAwait(false);
//        }

//        private void RegisterPersistTokenProvider()
//        {
//            if (ApplicationState == null) throw new NullReferenceException(nameof(ApplicationState));

//            if (TokenProvider != null)
//            {
//                persistingComponentStateSubscription = ApplicationState.RegisterOnPersisting(PersistTokenProvider);

//                if (ApplicationState.TryTakeFromJson<TokenProvider>($"{GetType().Name}-{PersistState.TOKEN_PROVIDER}", out var tokenProvider))
//                {
//                    _tokenProvider = tokenProvider;
//                }
//                else
//                {
//                    _tokenProvider = TokenProvider;
//                }
//            }
//        }

//        private Task PersistTokenProvider()
//        {
//            if (ApplicationState == null) throw new NullReferenceException(nameof(ApplicationState));
//            if (TokenProvider == null) throw new NullReferenceException(nameof(TokenProvider));

//            if (!string.IsNullOrWhiteSpace(TokenProvider.AccessToken))
//            {
//                ApplicationState.PersistAsJson($"{GetType().Name}-{PersistState.TOKEN_PROVIDER}", TokenProvider);
//            }

//            return Task.CompletedTask;
//        }

//        void IDisposable.Dispose()
//        {
//            persistingComponentStateSubscription.Dispose();
//        }
//    }
//}
