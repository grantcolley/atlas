using Atlas.Blazor.Shared.Base;
using Atlas.Blazor.Shared.Constants;
using Atlas.Blazor.Shared.Helpers;
using Atlas.Blazor.Shared.Models;
using System.Linq.Expressions;
using static MudBlazor.CategoryTypes;

namespace Atlas.Blazor.Shared.Components.Mud
{
    public abstract class IconDropdownBase<T> : ModelPropertyRenderComponentBase<T> where T : class, new()
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
            base.OnInitialized();

            _iconOptionItems = IconHelper.GetIconOptionItems();
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

            selectedItem ??= _iconOptionItems.First();

            PropertyValue = selectedItem.Id;
        }
    }
}
