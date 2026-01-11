using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Shared.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Can't convert success result to problem");
            }

            return Results.Problem(
                statusCode: GetStatusCode(result.Error.Type),
                title: GetTitle(result.Error.Type),
                type: GetType(result.Error.Type),
                detail: result.Error.Description, // Use the Error Description as the main Detail
                extensions: new Dictionary<string, object?>
                {
                    { "errors", CreateErrorDictionary(result) }
                }
            );
        }

        private static Dictionary<string, string[]> CreateErrorDictionary(Result result)
        {
            if(result is IValidationResult validationResult)
            {
                return validationResult.Errors
                    .GroupBy(e => e.Code)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.Description).ToArray()
                    );
            }

            //return new Dictionary<string, string[]>
            //{
            //    { result.Error.Code, new[] { result.Error.Description } }
            //};
            return new Dictionary<string, string[]>();
        }

        private static int GetStatusCode(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

        private static string GetTitle(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => "Validation Error",
            ErrorType.NotFound => "Not Found",
            ErrorType.Conflict => "Conflict",
            _ => "An error occurred while processing your request."
        };

        private static string GetType(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Validation => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                ErrorType.NotFound => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                ErrorType.Conflict => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                _ => "https://tools.ietf.org/html/rfc9110#section-15.6.1"
            };
    }
}
