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
    public abstract class DropdownGenericBase<T, TItem> : ModelPropertyRenderComponentBase<T> 
        where T : class, new()
        where TItem : ModelBase, new()
    {
        [Inject]
        public IOptionsRequest? OptionsRequest { get; set; }

        protected IEnumerable<GenericItem<TItem>>? _optionItems;

        private IEnumerable<OptionsArg>? _optionsArgs;

        private GenericItem<TItem>? _selectedItem;

        private string? _displayField;

        public GenericItem<TItem>? SelectedItem
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

            OptionsArg? displayFieldArgs = _optionsArgs.FirstOptionsArgDefault(ElementParams.GENERIC_MODEL_DISPLAY_FIELD);
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

            var propertyInfo = PropertyInfoHelper.GetPropertyInfo(typeof(TItem), _displayField ?? throw new NullReferenceException(nameof(_displayField)));

            var result = await OptionsRequest.GetOptionItemsAsync<TItem>(_optionsArgs).ConfigureAwait(false);

            var items = GetResponse(result);

            if (propertyInfo != null
                && items != null
                && items.Any())
            {
                _optionItems = items.Select(oi => new GenericItem<TItem>(oi, propertyInfo));

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
