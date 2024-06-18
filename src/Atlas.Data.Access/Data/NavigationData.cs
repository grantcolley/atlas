using Atlas.Core.Models;
using Atlas.Data.Access.Base;
using Atlas.Data.Access.Context;
using Atlas.Data.Access.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public class NavigationData : AuthorisationData<NavigationData>, INavigationData
    {
        public NavigationData(ApplicationDbContext applicationDbContext, ILogger<NavigationData> logger)
            : base(applicationDbContext, logger)
        {
        }

        public async Task<IEnumerable<Module>> GetModulesAsync(CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Modules
                .AsNoTracking()
                .OrderBy(m => m.Order)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Module?> GetModuleAsync(int id, CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Modules
                .Include(m => m.Categories)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ModuleId.Equals(id), cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Module> CreateModuleAsync(Module module, CancellationToken cancellationToken)
        {
            Module newModule = new()
            {
                ModuleId = module.ModuleId,
                Name = module.Name,
                Order = module.Order,
                Permission = module.Permission,
                Icon = module.Icon
            };

            await _applicationDbContext.Modules
                .AddAsync(newModule, cancellationToken)
                .ConfigureAwait(false);

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return newModule;
        }

        public async Task<Module> UpdateModuleAsync(Module module, CancellationToken cancellationToken)
        {
            Module? existing = await _applicationDbContext.Modules
                .FirstOrDefaultAsync(m => m.ModuleId.Equals(module.ModuleId), cancellationToken)
                .ConfigureAwait(false) 
                ?? throw new NullReferenceException($"ModuleId {module.ModuleId} not found.");

            _applicationDbContext
                .Entry(existing)
                .CurrentValues.SetValues(module);

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return existing;
        }

        public async Task<int> DeleteModuleAsync(int id, CancellationToken cancellationToken)
        {
            Module? module = await _applicationDbContext.Modules
                .FirstAsync(m => m.ModuleId.Equals(id), cancellationToken)
                .ConfigureAwait(false) 
                ?? throw new NullReferenceException($"ModuleId {id} not found.");

            _applicationDbContext.Remove(module);

            return await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Categories
                .AsNoTracking()
                .Include(c => c.Module)
                .OrderBy(m => m.Order)
                .ThenBy(c => c.Order)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Category?> GetCategoryAsync(int id, CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Categories
                .Include(c => c.Module)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryId.Equals(id), cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            if (category.Module == null)
            {
                throw new NullReferenceException(
                    $"{nameof(category)} {nameof(category.Module)}");
            }

            Module? module = await _applicationDbContext.Modules
                .FirstOrDefaultAsync(m => m.ModuleId.Equals(category.Module.ModuleId), cancellationToken)
                .ConfigureAwait(false)
                ?? throw new NullReferenceException($"ModuleId {category.Module.ModuleId} not found.");

            Category newCategory = new()
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Order = category.Order,
                Icon = category.Icon,
                Permission = category.Permission,
                Module = module
            };

            await _applicationDbContext.Categories
                .AddAsync(newCategory, cancellationToken)
                .ConfigureAwait(false);

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return newCategory;
        }

        public async Task<Category> UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            if (category.Module == null)
            {
                throw new NullReferenceException($"CategoryId {category.CategoryId} doesn't have a module assigned.");
            }

            Category? existing = await _applicationDbContext.Categories
                .Include(c => c.Module)
                .FirstOrDefaultAsync(c => c.CategoryId.Equals(category.CategoryId), cancellationToken)
                .ConfigureAwait(false)
                ?? throw new NullReferenceException($"CategoryId {category.CategoryId} not found.");

            _applicationDbContext
                    .Entry(existing)
                    .CurrentValues.SetValues(category);

            if (existing.Module == null)
            {
                throw new NullReferenceException(nameof(existing.Module));
            }
            
            if(!category.Module.ModuleId.Equals(existing.Module.ModuleId))
            {
                existing.Module = category.Module;
            }

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return existing;
        }

        public async Task<int> DeleteCategoryAsync(int id, CancellationToken cancellationToken)
        {
            Category? category = await _applicationDbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId.Equals(id), cancellationToken)
                .ConfigureAwait(false) 
                ?? throw new NullReferenceException($"CategoryId {id} not found.");

            _applicationDbContext.Remove(category);

            return await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<MenuItem>> GetMenuItemsAsync(CancellationToken cancellationToken)
        {
            IEnumerable<Category> categories = await _applicationDbContext.Categories
                .AsNoTracking()
                .Include(c => c.Module)
                .OrderBy(m => m.Order)
                .ThenBy(c => c.Order)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            IEnumerable<MenuItem> menuItems = await _applicationDbContext.MenuItems
                .AsNoTracking()
                .Include(c => c.Category)
                .OrderBy(mi => mi.Order)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            var orderedMenuItems = (from c in categories
                                    join mi in menuItems on c.CategoryId equals mi.Category?.CategoryId
                                    select mi).ToList();

            return orderedMenuItems;
        }

        public async Task<MenuItem?> GetMenuItemAsync(int id, CancellationToken cancellationToken)
        {
            return await _applicationDbContext.MenuItems
                .AsNoTracking()
                .Include(mi => mi.Category)
                .FirstOrDefaultAsync(mi => mi.MenuItemId.Equals(id), cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MenuItem> CreateMenuItemAsync(MenuItem menuItem, CancellationToken cancellationToken)
        {
            if (menuItem.Category == null)
            {
                throw new NullReferenceException($"MenuItemId {menuItem.MenuItemId} doesn't have a category assigned.");
            }

            Category? category = await _applicationDbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId.Equals(menuItem.Category.CategoryId), cancellationToken)
                .ConfigureAwait(false);

            MenuItem newMenuItem = new()
            {
                MenuItemId = menuItem.MenuItemId,
                Name = menuItem.Name,
                Order = menuItem.Order,
                Icon = menuItem.Icon,
                NavigatePage = menuItem.NavigatePage,
                Permission = menuItem.Permission,
                Category = category
            };

            await _applicationDbContext.MenuItems
                .AddAsync(newMenuItem, cancellationToken)
                .ConfigureAwait(false);

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return newMenuItem;
        }

        public async Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem, CancellationToken cancellationToken)
        {
            if (menuItem.Category == null)
            {
                throw new NullReferenceException($"MenuItemId {menuItem.MenuItemId} doesn't have a category assigned.");
            }

            MenuItem? existing = await _applicationDbContext.MenuItems
                .Include(mi => mi.Category)
                .FirstOrDefaultAsync(mi => mi.MenuItemId.Equals(menuItem.MenuItemId), cancellationToken)
                .ConfigureAwait(false)
                ?? throw new NullReferenceException($"{nameof(menuItem)} MenuItemId {menuItem.MenuItemId} not found.");

            _applicationDbContext
                .Entry(existing)
                .CurrentValues.SetValues(menuItem);

            if (existing.Category == null)
            {
                throw new NullReferenceException(nameof(existing.Category));
            }
            
            if (!menuItem.Category.CategoryId.Equals(existing.Category.CategoryId))
            {
                existing.Category = menuItem.Category;
            }

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            return existing;
        }

        public async Task<int> DeleteMenuItemAsync(int id, CancellationToken cancellationToken)
        {
            MenuItem? menuItem = await _applicationDbContext.MenuItems
                .FirstOrDefaultAsync(mi => mi.MenuItemId.Equals(id), cancellationToken)
                .ConfigureAwait(false)
                ?? throw new NullReferenceException($"MenuItemId {id} not found.");

            _applicationDbContext.Remove(menuItem);

            return await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
