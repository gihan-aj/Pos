using Pos.Web.Shared.Errors;

namespace Pos.Web.Shared.Abstractions
{
    // Special Result type for Validations that carries multiple errors
    public sealed class ValidationResult : Result, IValidationResult
    {
        private ValidationResult(Error[] errors)
            : base(false, IValidationResult.ValidationError)
        {
            Errors = errors;
        }

        public Error[] Errors { get; }

        public static ValidationResult WithErrors(Error[] errors) => new(errors);
    }
}
