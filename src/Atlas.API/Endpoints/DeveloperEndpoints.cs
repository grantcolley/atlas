using Atlas.API.Interfaces;
using Atlas.Core.Exceptions;
using Atlas.Core.Logging.Interfaces;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;

namespace Atlas.API.Endpoints
{
    internal static class DeveloperEndpoints
    {
        internal static async Task<IResult> GetDatabaseStatus(IDeveloperData developerData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            string? user = null;

            try
            {
                if (!claimService.HasDeveloperClaim())
                {
                    return Results.Unauthorized();
                }

                user = claimService.GetClaim();

                DatabaseStatus? databaseStatus = await developerData.GetDatabaseStatusAsync(user, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(databaseStatus);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, user);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> CreateDatabase(IDeveloperData developerData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            string? user = null;

            try
            {
                if (!claimService.HasDeveloperClaim())
                {
                    return Results.Unauthorized();
                }

                user = claimService.GetClaim();

                DatabaseStatus? databaseStatus = await developerData.CreateDatabaseAsync(user, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(databaseStatus);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, user);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> SeedDatabase(IDeveloperData developerData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            string? user = null;

            try
            {
                if (!claimService.HasDeveloperClaim())
                {
                    return Results.Unauthorized();
                }

                user = claimService.GetClaim();

                DatabaseStatus? databaseStatus = await developerData.SeedDatabaseAsync(user, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(databaseStatus);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, user);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
