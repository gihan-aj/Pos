using Pos.Web.Shared.Errors;

namespace Pos.Web.Shared.Abstractions
{
    // Special Result type for Validations that carries multiple errors
    public class ValidationResult : Result, IValidationResult
    {
        private ValidationResult(Error[] errors)
            : base(false, IValidationResult.ValidationError)
        {
            Errors = errors;
        }

        public Error[] Errors { get; }

        public static ValidationResult WithErrors(Error[] errors) => new(errors);
    }

    public class ValidationResult<T> : Result<T>, IValidationResult
    {
        private ValidationResult(Error[] errors)
            : base(default, false, IValidationResult.ValidationError)
        {
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }

        public Error[] Errors { get; }

        public static ValidationResult<T> WithErrors(Error[] errors) => new(errors);
    }
}
