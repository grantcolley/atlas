using Atlas.API.Interfaces;
using Atlas.Core.Exceptions;
using Atlas.Core.Logging.Interfaces;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Atlas.API.Endpoints
{
    internal static class OptionsEndpoints
    {
        internal static async Task<IResult> GetOptions([FromBody] IEnumerable<OptionsArg> optionsArgs, IOptionsData optionsData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await optionsData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null)
                {
                    return Results.Unauthorized();
                }
                
                IEnumerable<OptionItem> optionItems = await optionsData.GetOptionsAsync(optionsArgs, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(optionItems);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetGenericOptions([FromBody] IEnumerable<OptionsArg> optionsArgs, IOptionsData optionsData, IClaimService claimService, ILogService logService, CancellationToken cancellationToken)
        {
            Authorisation? authorisation = null;

            try
            {
                authorisation = await optionsData.GetAuthorisationAsync(claimService.GetClaim(), cancellationToken)
                    .ConfigureAwait(false);

                if (authorisation == null)
                {
                    return Results.Unauthorized();
                }

                string genericOptions = await optionsData.GetGenericOptionsAsync(optionsArgs, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Text(genericOptions, "application/json", Encoding.UTF8);
            }
            catch (AtlasException ex)
            {
                logService.Log(Core.Logging.Enums.LogLevel.Error, ex.Message, ex, authorisation?.User);

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
