using Atlas.API.Interfaces;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.API.Endpoints
{
    internal static class ApplicationEndpoints
    {
        internal static async Task<IResult> GetModules(IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Module>? modules = await applicationData.GetModulesAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(new AuthResult<IEnumerable<Module>?> { Authorisation = authorisation, Result = modules });
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.GetModulesAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetModule(int id, IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Module? module = await applicationData.GetModuleAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(module);
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.GetModuleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreateModule([FromBody] Module module, IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Module? newModule = await applicationData.CreateModuleAsync(module, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newModule);
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.CreateModuleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdateModule([FromBody] Module module, IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Module? updatedModule = await applicationData.UpdateModuleAsync(module, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedModule);
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.UpdateModuleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeleteModule(int id, IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                int affectedRows = await applicationData.DeleteModuleAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(affectedRows);
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.DeleteModuleAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetCategories(IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Category>? categories = await applicationData.GetCategoriesAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(new AuthResult<IEnumerable<Category>?> { Authorisation = authorisation, Result = categories });
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.GetCategoriesAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetCategory(int id, IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Category? category = await applicationData.GetCategoryAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(category);
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.GetCategoryAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreateCategory([FromBody] Category category, IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Category? newCategory = await applicationData.CreateCategoryAsync(category, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newCategory);
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.CreateCategoryAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdateCategory([FromBody] Category category, IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Category? updatedCategory = await applicationData.UpdateCategoryAsync(category, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedCategory);
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.UpdateCategoryAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeleteCategory(int id, IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                int affectedRows = await applicationData.DeleteCategoryAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(affectedRows);
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.DeleteCategoryAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        internal static async Task<IResult> GetPages(IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Page>? pages = await applicationData.GetPagesAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(new AuthResult<IEnumerable<Page>?> { Authorisation = authorisation, Result = pages });
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.GetPagesAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetPage(int id, IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Page? page = await applicationData.GetPageAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(page);
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.GetPageAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreatePage([FromBody] Page page, IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Page? newPage = await applicationData.CreatePageAsync(page, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newPage);
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.CreatePageAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdatePage([FromBody] Page page, IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                Page? updatedPage = await applicationData.UpdatePageAsync(page, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedPage);
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.UpdatePageAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeletePage(int id, IApplicationData applicationData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                int affectedRows = await applicationData.DeletePageAsync(id, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(affectedRows);
            }
            catch (Exception)
            {
                // Exceptions thrown from applicationData.DeletePageAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
