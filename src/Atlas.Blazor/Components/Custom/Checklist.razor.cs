using Atlas.Blazor.Base;
using Atlas.Blazor.Constants;
using Atlas.Core.Extensions;
using Atlas.Core.Models;
using MudBlazor;

namespace Atlas.Blazor.Components.Custom
{
    public abstract class ChecklistBase<T> : ModelPropertyRenderComponentBase<T> where T : class, new()
    {
        protected Typo LabelTypo = Typo.inherit;

        protected string FilterString = string.Empty;

        protected bool HasLabel = false;

        private IEnumerable<OptionsArg>? _optionsArgs;

        protected bool FilterFunction(ChecklistItem item) => FilterItem(item, FilterString);

        protected List<ChecklistItem> _checklist = new();

        protected override Task OnParametersSetAsync()
        {
            if (ModelPropertyRender == null) throw new NullReferenceException(nameof(ModelPropertyRender));

            _optionsArgs = ModelPropertyRender.Parameters.ToOptionsArgs();

            OptionsArg? argLabelTypo = _optionsArgs.FirstOptionsArgDefault(ElementParams.LABEL_TYPO);

            if (argLabelTypo != null
                && !string.IsNullOrWhiteSpace(argLabelTypo.Value))
            {
                HasLabel = true;

                LabelTypo = (Typo)Enum.Parse(typeof(Typo), argLabelTypo.Value);
            }

            List<ChecklistItem>? list = (List<ChecklistItem>?)ModelPropertyRender?.GetValue();

            if (list != null)
            {
                _checklist = list;
            }

            return base.OnParametersSetAsync();
        }

        protected static void OnCheckItem(ChecklistItem item)
        {
            item.IsChecked = !item.IsChecked;
        }

        private static bool FilterItem(ChecklistItem item, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            if (item.Name != null
                && item.Name.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
