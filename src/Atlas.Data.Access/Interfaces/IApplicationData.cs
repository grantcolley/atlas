using Atlas.Core.Models;

namespace Atlas.Data.Access.Interfaces
{
    public interface IApplicationData : IAuthorisationData
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
        Task<IEnumerable<Page>> GetPagesAsync(CancellationToken cancellationToken);
        Task<Page?> GetPageAsync(int id, CancellationToken cancellationToken);
        Task<Page> CreatePageAsync(Page page, CancellationToken cancellationToken);
        Task<Page> UpdatePageAsync(Page page, CancellationToken cancellationToken);
        Task<int> DeletePageAsync(int id, CancellationToken cancellationToken);
    }
}
