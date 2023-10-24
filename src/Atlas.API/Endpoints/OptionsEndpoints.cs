using Atlas.API.Interfaces;
using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Data.Access.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.API.Endpoints
{
    internal static class OptionsEndpoints
    {
        internal static async Task<IResult> GetOptions([FromBody] IEnumerable<OptionsArg> optionsArgs, IOptionsData optionsData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await optionsData.GetAuthorisationAsync(claimService.GetClaim())
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.USER))
                {
                    return Results.Unauthorized();
                }

                IEnumerable<OptionItem> optionItems = await optionsData.GetOptionsAsync(optionsArgs, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(optionItems);
            }
            catch (Exception)
            {
                // Exceptions thrown from optionsData.GetOptionsAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        internal static async Task<IResult> GetGenericOptions([FromBody] IEnumerable<OptionsArg> optionsArgs, IOptionsData optionsData, IClaimService claimService, CancellationToken cancellationToken)
        {
            try
            {
                Authorisation? authorisation = await optionsData.GetAuthorisationAsync(claimService.GetClaim())
                    .ConfigureAwait(false);

                if (authorisation == null
                    || !authorisation.HasPermission(Auth.USER))
                {
                    return Results.Unauthorized();
                }

                string genericOptions = await optionsData.GetGenericOptionsAsync(optionsArgs, cancellationToken)
                    .ConfigureAwait(false);

                return Results.Ok(genericOptions);
            }
            catch (Exception)
            {
                // Exceptions thrown from optionsData.GetGenericOptionsAsync()
                // have already been logged so simply return Status500InternalServerError.

                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
