using Atlas.API.Interfaces;
using Atlas.Core.Constants;
using Atlas.Core.Exceptions;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.API.Endpoints
{
    internal static class SupportEndpoints
    {
        internal static async Task<IResult> GetLogs([FromBody] LogArgs logArgs, ISupportData supportData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await supportData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.SUPPORT))
                {
                    return Results.Unauthorized();
                }
                
                IEnumerable<Log> logs = await supportData.GetLogsAsync(logArgs, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(logs);
            }
            catch (AtlasException ex)
            {
                logService.Log(Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
