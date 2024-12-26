using Atlas.API.Interfaces;
using Atlas.Core.Exceptions;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using Atlas.Logging.Interfaces;

namespace Atlas.API.Endpoints
{
    internal static class ClaimEndpoints
    {
        internal static async Task<IResult> GetClaimAuthorisation(IClaimData claimData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await claimData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || string.IsNullOrWhiteSpace(authorisation.User))
                {
                    return Results.Unauthorized();
                }

                return Results.Ok(authorisation);
            }
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetClaimModules(IClaimData claimData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await claimData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);
            }
            catch(AtlasException ex)
            {
                if ((ex.Message.StartsWith("Cannot open database")
                    || ex.Message.Contains("unauthorized"))
                    && claimService.HasDeveloperClaim())
                {
                    logService.Log(Logging.Enums.LogLevel.Information, ex.Message, ex, authorisation?.User);

                    return Results.Ok(claimData.GetDeveloperDatabaseClaim());
                }
                else
                {
                    logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            try
            {
                if (authorisation == null)
                {
                    return Results.Unauthorized();
                }

                IEnumerable<Module>? modules = await claimData.GetApplicationClaimsAsync(cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(modules);
            }
            catch (AtlasException ex)
            {
                logService.Log(Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
