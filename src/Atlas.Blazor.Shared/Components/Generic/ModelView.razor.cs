﻿using Atlas.Blazor.Shared.Base;
using Atlas.Blazor.Shared.Constants;
using Atlas.Blazor.Shared.Models;
using Atlas.Core.Constants;
using Atlas.Core.Dynamic;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Atlas.Blazor.Shared.Components.Generic
{
    public abstract class ModelViewBase<T> : AtlasGenericComponentBase, IDisposable where T : class, new()
    {
        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string? IdentityField { get; set; }

        [Parameter]
        public string? TitleField { get; set; }

        [Parameter]
        public string? CreateEndpoint { get; set; }

        [Parameter]
        public string? UpdateEndpoint { get; set; }

        [Parameter]
        public string? DeleteEndpoint { get; set; }

        [Parameter]
        public RenderFragment? ModelBodyFragment { get; set; }

        protected EditContext? CurrentEditContext { get; set; }

        protected T? _model;

        protected DynamicType<T>? DynamicType;

        protected Alert? _alert;

        protected List<string> _messages = new();

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
            if (GenericRequests == null)
            {
                throw new ArgumentNullException(nameof(GenericRequests));
            }

            await base.OnInitializedAsync();

            DynamicType = DynamicTypeHelper.Get<T>();

            if (Id.Equals(0))
            {
                _model = Activator.CreateInstance<T>();
            }
            else
            {
                IResponse<T> response = await GenericRequests.GetModelAsync<T>(Id, AtlasAPIEndpoints.GET_MODULE)
                    .ConfigureAwait(false);

                if (response.IsSuccess)
                {
                    _model = response.Result;
                }
                else if (!string.IsNullOrWhiteSpace(response.Message))
                {
                    RaiseAlert(response.Message);
                }
            }

            CurrentEditContext = new EditContext(_model ?? throw new NullReferenceException(nameof(_model)));
            CurrentEditContext.OnValidationStateChanged += CurrentEditContextOnValidationStateChanged;
        }

        protected virtual async Task SubmitAsync()
        {
            if (_model == null)
            {
                throw new ArgumentNullException(nameof(_model));
            }

            if (IdentityField == null)
            {
                throw new ArgumentNullException(nameof(IdentityField));
            }

            if (DynamicType == null)
            {
                throw new ArgumentNullException(nameof(DynamicType));
            }

            if (GenericRequests == null)
            {
                throw new ArgumentNullException(nameof(GenericRequests));
            }

            if (CreateEndpoint == null)
            {
                throw new ArgumentNullException(nameof(GenericRequests));
            }

            if (UpdateEndpoint == null)
            {
                throw new ArgumentNullException(nameof(UpdateEndpoint));
            }

            _isSaveInProgress = true;

            if (CurrentEditContext != null
                && CurrentEditContext.Validate())
            {
                IResponse<T> response;

                int id = (int)DynamicType.GetValue(_model, IdentityField);

                if (id.Equals(0))
                {
                    response = await GenericRequests
                        .CreateModelAsync<T>(_model, CreateEndpoint)
                        .ConfigureAwait(false);
                }
                else
                {
                    response = await GenericRequests
                        .UpdateModelAsync<T>(_model, UpdateEndpoint)
                        .ConfigureAwait(false);
                }

                _ = GetResponse(response);
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
            if (_model == null)
            {
                throw new ArgumentNullException(nameof(_model));
            }

            if (IdentityField == null)
            {
                throw new ArgumentNullException(nameof(IdentityField));
            }

            if (TitleField == null)
            {
                throw new ArgumentNullException(nameof(TitleField));
            }

            if (DynamicType == null)
            {
                throw new ArgumentNullException(nameof(DynamicType));
            }

            if (GenericRequests == null)
            {
                throw new ArgumentNullException(nameof(GenericRequests));
            }

            if (DialogService == null)
            {
                throw new ArgumentNullException(nameof(DialogService));
            }

            if (DeleteEndpoint == null)
            {
                throw new ArgumentNullException(nameof(DeleteEndpoint));
            }

            int id = (int)DynamicType.GetValue(_model, IdentityField);

            if (id.Equals(0))
            {
                await DialogService.ShowAsync(
                    "Delete", $"Cannot delete an object with Id equal to 0",
                    "Close", false, Color.Warning, false)
                    .ConfigureAwait(false);
                return;
            }

            string? title = DynamicType.GetValue(_model, TitleField).ToString();

            var deleteResult = await DialogService.ShowAsync(
                "Delete", $"Do you really want to delete {title}",
                "Delete", true, Color.Warning, false)
                .ConfigureAwait(false);

            if (deleteResult.Canceled)
            {
                return;
            }

            _isDeleteInProgress = true;

            var response = await GenericRequests
                .DeleteModelAsync(id, DeleteEndpoint)
                .ConfigureAwait(false);

            var result = GetResponse(response);

            if (result.Equals(0))
            {
                return;
            }

            _alert = new Alert
            {
                AlertType = Alerts.INFO,
                Title = TitleField,
                Message = $"has been deleted."
            };

            _isDeleteInProgress = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (CurrentEditContext != null)
                    {
                        CurrentEditContext.OnValidationStateChanged -= CurrentEditContextOnValidationStateChanged;
                    }
                }

                CurrentEditContext = null;

                disposedValue = true;
            }
        }

        private void CurrentEditContextOnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
        {
            GetValidationMessages();
        }
    }
}