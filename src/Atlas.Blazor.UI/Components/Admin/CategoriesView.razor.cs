using Atlas.Blazor.UI.Base;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Interfaces;

namespace Atlas.Blazor.UI.Components.Admin
{
    public abstract class CategoriesViewBase : GenericPageArgsBase
    {
        protected IEnumerable<Category>? _categories;
        protected bool _loaded = false;

        protected override async Task OnInitializedAsync()
        {
            if (GenericRequests == null) throw new ArgumentNullException(nameof(GenericRequests));

            await base.OnInitializedAsync().ConfigureAwait(false);

            IResponse<IEnumerable<Category>> response = await GenericRequests.GetListAsync<Category>(AtlasAPIEndpoints.GET_CATEGORIES)
                .ConfigureAwait(false);

            if(response.IsSuccess)
            {
                _categories = response.Result;
            }
            else if(!string.IsNullOrWhiteSpace(response.Message))
            {
                RaiseAlert(response.Message);
            }

            _loaded = true;
        }
    }
}
