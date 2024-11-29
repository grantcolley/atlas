using Atlas.Core.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace Atlas.API.Utility
{
    public static class ValidationExceptionHelper
    {
        public static async Task ValidateAndThrowAtlasException<T>(this IValidator<T> validator, T model, string location, CancellationToken cancellationToken)
        {
            ValidationResult validationResult = await validator.ValidateAsync(model, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new AtlasException($"{location} : {validationResult.ToString(",")}");
            }
        }
    }
}
