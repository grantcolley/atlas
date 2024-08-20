using Atlas.API.Interfaces;
using Atlas.Core.Constants;
using Atlas.Core.Exceptions;
using Atlas.Core.Logging.Interfaces;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Atlas.API.Endpoints
{
    internal static class SupportEndpoints
    {
        internal static async Task<IResult> GetLogs([FromBody] string args, ISupportData supportData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(args, nameof(args));

                LogArgs? logArgs = JsonSerializer.Deserialize<LogArgs>(args);

                ArgumentNullException.ThrowIfNull(logArgs, nameof(logArgs));

                authorisation = await supportData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.SUPPORT))
                {
                    return Results.Unauthorized();
                }
              
                IEnumerable<Log> logs = await supportData.GetLogsAsync(logArgs, cancellationToken)
                .ConfigureAwait(false);

                return Results.Ok(new AuthResult<IEnumerable<Log>?> { Authorisation = authorisation, Result = logs });
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
