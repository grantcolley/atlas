using Atlas.Blazor.Base;
using Atlas.Blazor.Constants;
using Atlas.Blazor.Helpers;
using Atlas.Blazor.Models;
using Atlas.Core.Extensions;
using Atlas.Core.Models;
using System.Linq.Expressions;

namespace Atlas.Blazor.Components.Mud
{
    public abstract class DropdownIconBase<T> : ModelPropertyRenderComponentBase<T> where T : class, new()
    {
        protected IEnumerable<IconOptionItem>? _iconOptionItems;

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
                else
                {
                    ModelPropertyRender?.SetValue(value);
                }
            }
        }

        protected override void OnInitialized()
        {
            if (ModelPropertyRender == null) throw new NullReferenceException(nameof(ModelPropertyRender));

            base.OnInitialized();

            IEnumerable<OptionsArg> optionsArgs = ModelPropertyRender.Parameters.ToOptionsArgs();

            OptionsArg? iconsArgs = optionsArgs.FirstOptionsArgDefault(IconsOptions.ICONS_CODE);

            if (iconsArgs == null || string.IsNullOrWhiteSpace(iconsArgs.Value)) throw new NullReferenceException(nameof(iconsArgs));

            _iconOptionItems = IconHelper.GetIconOptionItems(iconsArgs.Value);
        }

        protected override async Task OnParametersSetAsync()
        {
            if (ModelPropertyRender == null) throw new NullReferenceException(nameof(ModelPropertyRender));

            await base.OnParametersSetAsync().ConfigureAwait(false);

            if(_iconOptionItems == null)
            {
                return;
            }

            IconOptionItem? selectedItem = null;

            string? id = ModelPropertyRender?.GetValue()?.ToString();

            if (!string.IsNullOrWhiteSpace(id))
            {
                selectedItem = _iconOptionItems.FirstOrDefault(o => o.Id != null && o.Id.Equals(id));
            }

            PropertyValue = selectedItem?.Id;
        }
    }
}
