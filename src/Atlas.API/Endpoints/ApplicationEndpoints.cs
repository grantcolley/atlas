using Atlas.API.Interfaces;
using Atlas.API.Utility;
using Atlas.Core.Constants;
using Atlas.Core.Exceptions;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using Atlas.Logging.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.API.Endpoints
{
    internal static class ApplicationEndpoints
    {
        internal static async Task<IResult> GetModules(IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
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
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetModule(int id, IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
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
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreateModule([FromBody] Module module, IValidator<Module> validator, IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                await validator.ValidateAndThrowAtlasException(module, "CreateModule", cancellationToken).ConfigureAwait(false);

                Module? newModule = await applicationData.CreateModuleAsync(module, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newModule);
            }
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdateModule([FromBody] Module module, IValidator<Module> validator, IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                await validator.ValidateAndThrowAtlasException(module, "UpdateModule", cancellationToken).ConfigureAwait(false);

                Module? updatedModule = await applicationData.UpdateModuleAsync(module, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedModule);
            }
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeleteModule(int id, IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
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
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetCategories(IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
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
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetCategory(int id, IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
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
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreateCategory([FromBody] Category category, IValidator<Category> validator, IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                await validator.ValidateAndThrowAtlasException(category, "CreateCategory", cancellationToken).ConfigureAwait(false);

                Category? newCategory = await applicationData.CreateCategoryAsync(category, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newCategory);
            }
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdateCategory([FromBody] Category category, IValidator<Category> validator, IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                await validator.ValidateAndThrowAtlasException(category, "UpdateCategory", cancellationToken).ConfigureAwait(false);

                Category? updatedCategory = await applicationData.UpdateCategoryAsync(category, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedCategory);
            }
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeleteCategory(int id, IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
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
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        internal static async Task<IResult> GetPages(IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
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
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetPage(int id, IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
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
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreatePage([FromBody] Page page, IValidator<Page> validator, IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                await validator.ValidateAndThrowAtlasException(page, "CreatePage", cancellationToken).ConfigureAwait(false);

                Page? newPage = await applicationData.CreatePageAsync(page, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(newPage);
            }
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> UpdatePage([FromBody] Page page, IValidator<Page> validator, IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.DEVELOPER))
                {
                    return Results.Unauthorized();
                }

                await validator.ValidateAndThrowAtlasException(page, "UpdatePage", cancellationToken).ConfigureAwait(false);

                Page? updatedPage = await applicationData.UpdatePageAsync(page, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(updatedPage);
            }
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> DeletePage(int id, IApplicationData applicationData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await applicationData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
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
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
