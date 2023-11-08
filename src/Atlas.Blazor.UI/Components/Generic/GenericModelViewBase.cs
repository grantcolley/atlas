using Atlas.Blazor.UI.Base;
using Atlas.Blazor.UI.Constants;
using Atlas.Blazor.UI.Models;
using Atlas.Blazor.UI.Render;
using Atlas.Core.Dynamic;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Atlas.Blazor.UI.Components.Generic
{
    public abstract class GenericModelViewBase<T, TRender> : GenericPageArgsBase, IDisposable
        where T : class, new()
        where TRender : ModelRender<T>, new()
    {
        [Parameter]
        public string? IdentityField { get; set; }

        [Parameter]
        public string? TitleField { get; set; }

        [Parameter]
        public string? GetEndpoint { get; set; }

        [Parameter]
        public string? CreateEndpoint { get; set; }

        [Parameter]
        public string? UpdateEndpoint { get; set; }

        [Parameter]
        public string? DeleteEndpoint { get; set; }

        protected EditContext? CurrentEditContext { get; set; }

        protected T? _model;

        protected DynamicType<T>? DynamicType;

        protected Alert? _alert;

        protected List<string> _messages = new();

        protected string? _title;

        protected bool _isSaveInProgress = false;

        protected bool _isDeleteInProgress = false;

        protected bool _isReadOnly = false;

        private bool disposedValue;

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected override async Task OnInitializedAsync()
        {
            if (GenericRequests == null) throw new ArgumentNullException(nameof(GenericRequests));
            if (GetEndpoint == null) throw new ArgumentNullException(nameof(GetEndpoint));
            if (PageArgs == null) throw new ArgumentNullException(nameof(PageArgs));

            await base.OnInitializedAsync();

            DynamicType = DynamicTypeHelper.Get<T>();

            string? breadcrumbText = null;

            if (PageArgs.ModelInstanceId.Equals(0))
            {
                _model = Activator.CreateInstance<T>();

                breadcrumbText = $"New {typeof(T).Name}";
            }
            else
            {
                IResponse<T> response = await GenericRequests.GetModelAsync<T>(PageArgs.ModelInstanceId, GetEndpoint)
                    .ConfigureAwait(false);

                if (response.IsSuccess)
                {
                    _model = response.Result;

                    if(_model != null
                        && !string.IsNullOrWhiteSpace(TitleField)) 
                    {
                        breadcrumbText = $"{PageArgs.ModelInstanceId} {DynamicType.GetValue(_model, TitleField)?.ToString()}";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(response.Message))
                {
                    RaiseAlert(response.Message);
                }
            }

            await SendBreadcrumbAsync(BreadcrumbAction.Add, breadcrumbText)
                .ConfigureAwait(false);

            CreateEditContext();
        }

        protected virtual async Task SubmitAsync()
        {
            if (_model == null) throw new NullReferenceException(nameof(_model));
            if (IdentityField == null) throw new NullReferenceException(nameof(IdentityField));
            if (TitleField == null) throw new NullReferenceException(nameof(IdentityField));
            if (DynamicType == null) throw new NullReferenceException(nameof(DynamicType));
            if (GenericRequests == null) throw new NullReferenceException(nameof(GenericRequests));
            if (CreateEndpoint == null) throw new NullReferenceException(nameof(GenericRequests));
            if (UpdateEndpoint == null) throw new NullReferenceException(nameof(UpdateEndpoint));
            if (StateNotificationService == null) throw new NullReferenceException(nameof(StateNotificationService));

            _isSaveInProgress = true;

            if (CurrentEditContext != null
                && CurrentEditContext.Validate())
            {
                IResponse<T> response;

                int? id = (int?)DynamicType.GetValue(_model, IdentityField);

                if (id.HasValue == false
                    || id.Equals(0))
                {
                    response = await GenericRequests
                        .CreateModelAsync<T>(_model, CreateEndpoint)
                        .ConfigureAwait(false);

                    _model = GetResponse(response);

                    if (_model != null)
                    {
                        string newId = $"{DynamicType.GetValue(_model, IdentityField)?.ToString()}";
                        string? href = NavigationManager?.Uri.Remove(0, NavigationManager.BaseUri.Length - 1);

                        await SendBreadcrumbAsync(
                            BreadcrumbAction.Update, 
                            $"{newId} {DynamicType.GetValue(_model, TitleField)?.ToString()}",
                            $"{href?.Remove(href.LastIndexOf("/") + 1)}{newId}")
                            .ConfigureAwait(false);
                    }
                }
                else
                {
                    response = await GenericRequests
                        .UpdateModelAsync<T>(_model, UpdateEndpoint)
                        .ConfigureAwait(false);

                    _model = GetResponse(response);
                }

                if(_model != null) 
                {
                    CreateEditContext();
                }
            }

            _isSaveInProgress = false;
        }

        protected virtual void GetValidationMessages()
        {
            if (CurrentEditContext != null)
            {
                var before = _messages.Count;

                _messages.Clear();
                _messages.AddRange(CurrentEditContext.GetValidationMessages());

                var after = _messages.Count;

                if (before != after)
                {
                    StateHasChanged();
                }
            }
        }

        protected virtual async Task DeleteAsync()
        {
            if (_model == null) throw new ArgumentNullException(nameof(_model));
            if (IdentityField == null) throw new ArgumentNullException(nameof(IdentityField));
            if (DynamicType == null) throw new ArgumentNullException(nameof(DynamicType));
            if (GenericRequests == null) throw new ArgumentNullException(nameof(GenericRequests));
            if (DialogService == null) throw new ArgumentNullException(nameof(DialogService));
            if (DeleteEndpoint == null) throw new ArgumentNullException(nameof(DeleteEndpoint));

            int? id = (int?)DynamicType.GetValue(_model, IdentityField);

            if (id.HasValue == false
                || id.Equals(0))
            {
                await DialogService.ShowAsync(
                    "Delete", $"Cannot delete an object with Id equal to 0 or null",
                    "Close", false, Color.Warning, false)
                    .ConfigureAwait(false);
                return;
            }

            var deleteResult = await DialogService.ShowAsync(
                "Delete", $"Do you really want to delete {_title}",
                "Delete", true, Color.Warning, false)
                .ConfigureAwait(false);

            if (deleteResult.Canceled)
            {
                return;
            }

            _isDeleteInProgress = true;

            var response = await GenericRequests
                .DeleteModelAsync(id.Value, DeleteEndpoint)
                .ConfigureAwait(false);

            var result = GetResponse(response);

            if (result.Equals(0))
            {
                return;
            }

            _alert = new Alert
            {
                AlertType = Alerts.INFO,
                Title = _title,
                Message = $"has been deleted."
            };

            await SendBreadcrumbAsync(BreadcrumbAction.RemoveLast)
                .ConfigureAwait(false);

            _isDeleteInProgress = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeEditContext();
                }

                CurrentEditContext = null;

                disposedValue = true;
            }
        }

        private void CurrentEditContextOnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
        {
            GetValidationMessages();
        }

        private void DisposeEditContext()
        {
            if (CurrentEditContext != null)
            {
                CurrentEditContext.OnValidationStateChanged -= CurrentEditContextOnValidationStateChanged;
                CurrentEditContext = null;
            }
        }

        private void CreateEditContext()
        {
            if (TitleField == null) throw new NullReferenceException(nameof(TitleField));
            if (IdentityField == null) throw new NullReferenceException(nameof(IdentityField));

            DisposeEditContext();

            CurrentEditContext = new EditContext(_model ?? throw new NullReferenceException(nameof(_model)));
            CurrentEditContext.OnValidationStateChanged += CurrentEditContextOnValidationStateChanged;

            int? id = (int?)DynamicType?.GetValue(_model, IdentityField);

            if (!id.HasValue || id.Value.Equals(0))
            {
                _title = $"New {typeof(T).Name}";
            }
            else
            {
                _title = $"{id} {DynamicType?.GetValue(_model, TitleField)?.ToString()}";
            }
        }
    }
}
