using FluentValidation.Results;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Shared.Extensions
{
    public static class ValidationExtensions
    {
        public static Error[] ToErrors(this ValidationResult validationResult)
        {
            return validationResult.Errors
                .Select(x => Error.Validation(x.PropertyName, x.ErrorMessage))
                .ToArray();
        }
    }
}
