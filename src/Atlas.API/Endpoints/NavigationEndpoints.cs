using Atlas.API.Interfaces;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.API.Endpoints
{
    internal static class NavigationEndpoints
    {
        internal static async Task<IResult> GetModules(INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Module>? modules = await navigationData.GetModulesAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(new AuthResult<IEnumerable<Module>?> { Authorisation = authorisation, Result = modules });
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.GetModulesAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetModule(int id, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Module? module = await navigationData.GetModuleAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(module);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.GetModuleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreateModule([FromBody] Module module, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Module? newModule = await navigationData.CreateModuleAsync(module, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newModule);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.CreateModuleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdateModule([FromBody] Module module, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Module? updatedModule = await navigationData.UpdateModuleAsync(module, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedModule);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.UpdateModuleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeleteModule(int id, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                int affectedRows = await navigationData.DeleteModuleAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(affectedRows);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.DeleteModuleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetCategories(INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Category>? categories = await navigationData.GetCategoriesAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(new AuthResult<IEnumerable<Category>?> { Authorisation = authorisation, Result = categories });
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.GetCategoriesAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetCategory(int id, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Category? category = await navigationData.GetCategoryAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(category);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.GetCategoryAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreateCategory([FromBody] Category category, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Category? newCategory = await navigationData.CreateCategoryAsync(category, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newCategory);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.CreateCategoryAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdateCategory([FromBody] Category category, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Category? updatedCategory = await navigationData.UpdateCategoryAsync(category, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedCategory);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.UpdateCategoryAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeleteCategory(int id, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                int affectedRows = await navigationData.DeleteCategoryAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(affectedRows);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.DeleteCategoryAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        internal static async Task<IResult> GetPages(INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Page>? pages = await navigationData.GetPagesAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(new AuthResult<IEnumerable<Page>?> { Authorisation = authorisation, Result = pages });
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.GetPagesAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetPage(int id, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Page? page = await navigationData.GetPageAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(page);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.GetPageAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreatePage([FromBody] Page page, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Page? newPage = await navigationData.CreatePageAsync(page, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newPage);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.CreatePageAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdatePage([FromBody] Page page, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Page? updatedPage = await navigationData.UpdatePageAsync(page, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedPage);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.UpdatePageAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeletePage(int id, INavigationData navigationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await navigationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                int affectedRows = await navigationData.DeletePageAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(affectedRows);
            }
            catch (Exception)
            {
                // Exceptions thrown from navigationData.DeletePageAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
