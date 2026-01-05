using Pos.Web.Shared.Errors;

namespace Pos.Web.Shared.Abstractions
{
    // Interface to mark validation results easily
    public interface IValidationResult
    {
        public static readonly Error ValidationError = new(
            "ValidationError",
            "One or more validation problems occurred.",
            ErrorType.Validation);

        Error[] Errors { get; }
    }
}
