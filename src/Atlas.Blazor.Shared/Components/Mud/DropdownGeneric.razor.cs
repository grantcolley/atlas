using Atlas.Blazor.Shared.Base;
using Atlas.Blazor.Shared.Constants;
using Atlas.Blazor.Shared.Models;
using Atlas.Core.Dynamic;
using Atlas.Core.Extensions;
using Atlas.Core.Models;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Atlas.Blazor.Shared.Components.Mud
{
    public abstract class DropdownGenericBase<T> : ModelPropertyRenderComponentBase<T> where T : class, new()
    {
        [Inject]
        public IOptionsRequest? OptionsRequest { get; set; }

        protected IEnumerable<GenericItem<T>>? _optionItems;

        private IEnumerable<OptionsArg>? _optionsArgs;

        private GenericItem<T>? _selectedItem;

        private string? _displayField;

        public GenericItem<T>? SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (value == null)
                {
                    _selectedItem = value;
                    ModelPropertyRender?.SetValue(null);
                }
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    ModelPropertyRender?.SetValue(SelectedItem?.Item);
                }
            }
        }

        protected override void OnInitialized()
        {
            if (ModelPropertyRender == null) throw new NullReferenceException(nameof(ModelPropertyRender));

            _optionsArgs = ModelPropertyRender.Parameters.ToOptionsArgs();

            OptionsArg? displayFieldArgs = _optionsArgs.FirstOptionsArgDefault(ElementParams.DISPLAY_FIELD);
            {
                if (displayFieldArgs != null
                    && !string.IsNullOrWhiteSpace(displayFieldArgs.Value))
                {
                    _displayField = displayFieldArgs.Value;
                }
            }

            base.OnInitialized();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (OptionsRequest == null) throw new NullReferenceException(nameof(OptionsRequest));
            if (_optionsArgs == null) throw new NullReferenceException(nameof(_optionsArgs));

            await base.OnParametersSetAsync().ConfigureAwait(false);

            var propertyInfo = PropertyInfoHelper.GetPropertyInfo(typeof(T), _displayField ?? throw new NullReferenceException(nameof(_displayField)));

            var result = await OptionsRequest.GetOptionItemsAsync<T>(_optionsArgs).ConfigureAwait(false);

            var items = GetResponse(result);

            if (items != null
                && items.Any())
            {
                _optionItems = items.Select(oi => new GenericItem<T>(oi, propertyInfo));

                if (ModelPropertyRender?.GetValue() != null)
                {
                    var value = propertyInfo?.GetValue(ModelPropertyRender.GetValue());
                    if (value != null)
                    {
                        _selectedItem = _optionItems.FirstOrDefault(
                            oi => oi.Name != null && oi.Name.Equals(value));
                    }
                }
            }

            await base.OnParametersSetAsync().ConfigureAwait(false);
        }
    }
}
