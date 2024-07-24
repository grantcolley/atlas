using Atlas.Core.Constants;
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
            Module module = await _applicationDbContext.Modules
                .Include(m => m.Categories)
                .AsNoTracking()
                .FirstAsync(m => m.ModuleId.Equals(id), cancellationToken)
                .ConfigureAwait(false);

            if (Authorisation == null
                || !Authorisation.HasPermission(Auth.DEVELOPER))
            {
                module.IsReadOnly = true;
            }

            return module;
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

            if (Authorisation == null
                || !Authorisation.HasPermission(Auth.DEVELOPER))
            {
                newModule.IsReadOnly = true;
            }

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

            if (Authorisation == null
                || !Authorisation.HasPermission(Auth.DEVELOPER))
            {
                existing.IsReadOnly = true;
            }

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
            Category category = await _applicationDbContext.Categories
                .Include(c => c.Module)
                .Include(c => c.Pages)
                .AsNoTracking()
                .FirstAsync(c => c.CategoryId.Equals(id), cancellationToken)
                .ConfigureAwait(false);

            if (Authorisation == null
                || !Authorisation.HasPermission(Auth.DEVELOPER))
            {
                category.IsReadOnly = true;
            }

            return category;
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

            if (Authorisation == null
                || !Authorisation.HasPermission(Auth.DEVELOPER))
            {
                newCategory.IsReadOnly = true;
            }

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

            if (Authorisation == null
                || !Authorisation.HasPermission(Auth.DEVELOPER))
            {
                existing.IsReadOnly = true;
            }

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

        public async Task<IEnumerable<Page>> GetPagesAsync(CancellationToken cancellationToken)
        {
            IEnumerable<Category> categories = await _applicationDbContext.Categories
                .AsNoTracking()
                .Include(c => c.Module)
                .OrderBy(m => m.Order)
                .ThenBy(c => c.Order)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            IEnumerable<Page> pages = await _applicationDbContext.Pages
                .AsNoTracking()
                .Include(c => c.Category)
                .OrderBy(mi => mi.Order)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            var orderedPages = (from c in categories
                                    join mi in pages on c.CategoryId equals mi.Category?.CategoryId
                                    select mi).ToList();

            return orderedPages;
        }

        public async Task<Page?> GetPageAsync(int id, CancellationToken cancellationToken)
        {
            Page page = await _applicationDbContext.Pages
                .AsNoTracking()
                .Include(mi => mi.Category)
                .FirstAsync(mi => mi.PageId.Equals(id), cancellationToken)
                .ConfigureAwait(false);

            if (Authorisation == null
                || !Authorisation.HasPermission(Auth.DEVELOPER))
            {
                page.IsReadOnly = true;
            }

            return page;
        }

        public async Task<Page> CreatePageAsync(Page page, CancellationToken cancellationToken)
        {
            if (page.Category == null)
            {
                throw new NullReferenceException($"PageId {page.PageId} doesn't have a category assigned.");
            }

            Category? category = await _applicationDbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId.Equals(page.Category.CategoryId), cancellationToken)
                .ConfigureAwait(false);

            Page newPage = new()
            {
                PageId = page.PageId,
                Name = page.Name,
                Order = page.Order,
                Icon = page.Icon,
                Route = page.Route,
                Permission = page.Permission,
                Category = category
            };

            await _applicationDbContext.Pages
                .AddAsync(newPage, cancellationToken)
                .ConfigureAwait(false);

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            if (Authorisation == null
                || !Authorisation.HasPermission(Auth.DEVELOPER))
            {
                newPage.IsReadOnly = true;
            }

            return newPage;
        }

        public async Task<Page> UpdatePageAsync(Page page, CancellationToken cancellationToken)
        {
            if (page.Category == null)
            {
                throw new NullReferenceException($"PageId {page.PageId} doesn't have a category assigned.");
            }

            Page? existing = await _applicationDbContext.Pages
                .Include(mi => mi.Category)
                .FirstOrDefaultAsync(mi => mi.PageId.Equals(page.PageId), cancellationToken)
                .ConfigureAwait(false)
                ?? throw new NullReferenceException($"{nameof(page)} PageId {page.PageId} not found.");

            _applicationDbContext
                .Entry(existing)
                .CurrentValues.SetValues(page);

            if (existing.Category == null)
            {
                throw new NullReferenceException(nameof(existing.Category));
            }
            
            if (!page.Category.CategoryId.Equals(existing.Category.CategoryId))
            {
                existing.Category = page.Category;
            }

            await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);

            if (Authorisation == null
                || !Authorisation.HasPermission(Auth.DEVELOPER))
            {
                existing.IsReadOnly = true;
            }

            return existing;
        }

        public async Task<int> DeletePageAsync(int id, CancellationToken cancellationToken)
        {
            Page? page = await _applicationDbContext.Pages
                .FirstOrDefaultAsync(mi => mi.PageId.Equals(id), cancellationToken)
                .ConfigureAwait(false)
                ?? throw new NullReferenceException($"PageId {id} not found.");

            _applicationDbContext.Remove(page);

            return await _applicationDbContext
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
