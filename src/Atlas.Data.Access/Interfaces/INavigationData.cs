using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface INavigationData : IAuthorisationData
    {
        Task<IEnumerable<Module>> GetModulesAsync(CancellationToken cancellationToken);
        Task<Module?> GetModuleAsync(int id, CancellationToken cancellationToken);
        Task<Module> CreateModuleAsync(Module module, CancellationToken cancellationToken);
        Task<Module> UpdateModuleAsync(Module module, CancellationToken cancellationToken);
        Task<int> DeleteModuleAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken);
        Task<Category?> GetCategoryAsync(int id, CancellationToken cancellationToken);
        Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken);
        Task<Category> UpdateCategoryAsync(Category category, CancellationToken cancellationToken);
        Task<int> DeleteCategoryAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<MenuItem>> GetMenuItemsAsync(CancellationToken cancellationToken);
        Task<MenuItem?> GetMenuItemAsync(int id, CancellationToken cancellationToken);
        Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem, CancellationToken cancellationToken);
        Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem, CancellationToken cancellationToken);
        Task<int> DeleteMenuItemAsync(int id, CancellationToken cancellationToken);
    }
}
