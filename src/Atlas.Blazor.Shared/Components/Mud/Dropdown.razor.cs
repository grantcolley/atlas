using Atlas.Blazor.Shared.Base;
using Atlas.Blazor.Shared.Constants;
using Atlas.Core.Extensions;
using Atlas.Core.Models;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Atlas.Blazor.Shared.Components.Mud
{
    public abstract class DropdownBase<T> : ModelPropertyRenderComponentBase<T> where T : class, new()
    {
        [Inject]
        public IOptionsRequest? OptionsRequest { get; set; }

        protected IEnumerable<OptionItem>? _optionItems;

        private IEnumerable<OptionsArg>? _optionsArgs;

        private bool _isNumericId = false;

        public Expression<Func<string?>> FieldExpression
        {
            get
            {
                if (ModelPropertyRender == null) throw new NullReferenceException(nameof(ModelPropertyRender));

                return Expression.Lambda<Func<string?>>(ModelPropertyRender.MemberExpression);
            }
        }

        public string? PropertyValue
        {
            get
            {
                return ModelPropertyRender?.GetValue()?.ToString();
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    ModelPropertyRender?.SetValue(null);
                }
                else if (_isNumericId)
                {
                    ModelPropertyRender?.SetValue(int.Parse(value));
                }
                else
                {
                    ModelPropertyRender?.SetValue(value);
                }
            }
        }

        protected override void OnInitialized()
        {
            if (ModelPropertyRender == null) throw new NullReferenceException(nameof(ModelPropertyRender));

            _optionsArgs = ModelPropertyRender.Parameters.ToOptionsArgs();

            OptionsArg? isNumericIdArgs = _optionsArgs.FirstOptionsArgDefault(ElementParams.IS_NUMERIC_ID);
            {
                if (isNumericIdArgs != null
                    && !string.IsNullOrWhiteSpace(isNumericIdArgs.Value))
                {
                    _isNumericId = bool.Parse(isNumericIdArgs.Value);
                }
            }

            base.OnInitialized();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (OptionsRequest == null) throw new NullReferenceException(nameof(OptionsRequest));
            if (_optionsArgs == null) throw new NullReferenceException(nameof(_optionsArgs));

            await base.OnParametersSetAsync().ConfigureAwait(false);

            var result = await OptionsRequest.GetOptionItems(_optionsArgs).ConfigureAwait(false);

            _optionItems = GetResponse(result);

            if(_optionItems == null)
            {
                return;
            }

            OptionItem? selectedItem = null;

            if (_isNumericId)
            {
                int? id = (int?)ModelPropertyRender?.GetValue();
                if (id.HasValue)
                {
                    selectedItem = _optionItems.FirstOrDefault(o => o.Id != null && o.Id.Equals(id.Value.ToString()));
                }
            }
            else
            {
                string? id = ModelPropertyRender?.GetValue()?.ToString();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    selectedItem = _optionItems.FirstOrDefault(o => o.Id != null && o.Id.Equals(id));
                }
            }

            selectedItem ??= _optionItems.First();

            PropertyValue = selectedItem.Id;
        }
    }
}
