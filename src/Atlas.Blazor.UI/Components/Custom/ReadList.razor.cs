using Atlas.Blazor.UI.Base;
using Atlas.Blazor.UI.Constants;
using Atlas.Core.Extensions;
using Atlas.Core.Models;
using MudBlazor;

namespace Atlas.Blazor.UI.Components.Custom
{
    public abstract class ReadListBase<T> : ModelPropertyRenderComponentBase<T> where T : class, new()
    {
        protected Typo LabelTypo = Typo.inherit;

        protected string FilterString = string.Empty;

        protected bool HasLabel = false;

        private IEnumerable<OptionsArg>? _optionsArgs;

        protected bool FilterFunction(string item) => FilterItem(item, FilterString);

        protected List<string>? _list = new();

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

            List<string>? list = (List<string>?)ModelPropertyRender?.GetValue();

            if (list != null)
            {
                _list = list;
            }

            return base.OnParametersSetAsync();
        }

        private static bool FilterItem(string item, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }

            if (!string.IsNullOrWhiteSpace(item)
                && item.Contains(filter, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
