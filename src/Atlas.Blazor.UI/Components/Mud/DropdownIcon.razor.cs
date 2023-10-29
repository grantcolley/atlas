using Atlas.Blazor.UI.Base;
using Atlas.Blazor.UI.Constants;
using Atlas.Blazor.UI.Helpers;
using Atlas.Blazor.UI.Models;
using System.Linq.Expressions;

namespace Atlas.Blazor.UI.Components.Mud
{
    public abstract class DropdownBaseIcon<T> : ModelPropertyRenderComponentBase<T> where T : class, new()
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

            //selectedItem ??= _iconOptionItems.First();

            PropertyValue = selectedItem?.Id;
        }
    }
}
