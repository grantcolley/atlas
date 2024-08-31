using Atlas.Core.Constants;
using Atlas.Core.Exceptions;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using Atlas.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlas.Data.Access.Data
{
    public class ApplicationData(ApplicationDbContext applicationDbContext, ILogger<ApplicationData> logger)
        : AuthorisationData<ApplicationData>(applicationDbContext, logger), IApplicationData
    {
        public async Task<IEnumerable<Module>> GetModulesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _applicationDbContext.Modules
                    .AsNoTracking()
                    .OrderBy(m => m.Order)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex);
            }
        }

        public async Task<Module?> GetModuleAsync(int id, CancellationToken cancellationToken)
        {
            try
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
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"ModuleId={id}");
            }
        }

        public async Task<Module> CreateModuleAsync(Module module, CancellationToken cancellationToken)
        {
            try
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
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex);
            }
        }

        public async Task<Module> UpdateModuleAsync(Module module, CancellationToken cancellationToken)
        {
            try
            {
                Module existing = await _applicationDbContext.Modules
                    .FirstAsync(m => m.ModuleId.Equals(module.ModuleId), cancellationToken)
                    .ConfigureAwait(false);

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
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"ModuleId={module.ModuleId}");
            }
        }

        public async Task<int> DeleteModuleAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                Module module = await _applicationDbContext.Modules
                    .FirstAsync(m => m.ModuleId.Equals(id), cancellationToken)
                    .ConfigureAwait(false);

                _applicationDbContext.Remove(module);

                return await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"ModuleId={id}");
            }
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _applicationDbContext.Categories
                    .AsNoTracking()
                    .Include(c => c.Module)
                    .OrderBy(m => m.Order)
                    .ThenBy(c => c.Order)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex);
            }
        }

        public async Task<Category?> GetCategoryAsync(int id, CancellationToken cancellationToken)
        {
            try
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
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"CategoryId={id}");
            }
        }

        public async Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            try
            {
                if (category.Module == null)
                {
                    throw new NullReferenceException(
                        $"{nameof(category)} {nameof(category.Module)}");
                }

                Module module = await _applicationDbContext.Modules
                    .FirstAsync(m => m.ModuleId.Equals(category.Module.ModuleId), cancellationToken)
                    .ConfigureAwait(false);

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
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex);
            }
        }

        public async Task<Category> UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
        {
            try
            {
                if (category.Module == null)
                {
                    throw new NullReferenceException($"CategoryId {category.CategoryId} doesn't have a module assigned.");
                }

                Category existing = await _applicationDbContext.Categories
                    .Include(c => c.Module)
                    .FirstAsync(c => c.CategoryId.Equals(category.CategoryId), cancellationToken)
                    .ConfigureAwait(false);

                _applicationDbContext
                        .Entry(existing)
                        .CurrentValues.SetValues(category);

                if (existing.Module == null)
                {
                    throw new NullReferenceException(nameof(existing.Module));
                }

                if (!category.Module.ModuleId.Equals(existing.Module.ModuleId))
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
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"CategoryId={category.CategoryId}");
            }
        }

        public async Task<int> DeleteCategoryAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                Category category = await _applicationDbContext.Categories
                    .FirstAsync(c => c.CategoryId.Equals(id), cancellationToken)
                    .ConfigureAwait(false);

                _applicationDbContext.Remove(category);

                return await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"CategoryId={id}");
            }
        }

        public async Task<IEnumerable<Page>> GetPagesAsync(CancellationToken cancellationToken)
        {
            try
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

                List<Page> orderedPages = (from c in categories
                                           join mi in pages on c.CategoryId equals mi.Category?.CategoryId
                                           select mi).ToList();

                return orderedPages;
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex);
            }
        }

        public async Task<Page?> GetPageAsync(int id, CancellationToken cancellationToken)
        {
            try
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
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"PageId={id}");
            }
        }

        public async Task<Page> CreatePageAsync(Page page, CancellationToken cancellationToken)
        {
            try
            {
                if (page.Category == null)
                {
                    throw new NullReferenceException($"PageId {page.PageId} doesn't have a category assigned.");
                }

                Category category = await _applicationDbContext.Categories
                    .FirstAsync(c => c.CategoryId.Equals(page.Category.CategoryId), cancellationToken)
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
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex);
            }
        }

        public async Task<Page> UpdatePageAsync(Page page, CancellationToken cancellationToken)
        {
            try
            {
                if (page.Category == null)
                {
                    throw new NullReferenceException($"PageId {page.PageId} doesn't have a category assigned.");
                }

                Page existing = await _applicationDbContext.Pages
                    .Include(mi => mi.Category)
                    .FirstAsync(mi => mi.PageId.Equals(page.PageId), cancellationToken)
                    .ConfigureAwait(false);

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
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"PageId={page.PageId}");
            }
        }

        public async Task<int> DeletePageAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                Page page = await _applicationDbContext.Pages
                    .FirstAsync(mi => mi.PageId.Equals(id), cancellationToken)
                    .ConfigureAwait(false);

                _applicationDbContext.Remove(page);

                return await _applicationDbContext
                    .SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AtlasException(ex.Message, ex, $"PageId={id}");
            }
        }
    }
}
