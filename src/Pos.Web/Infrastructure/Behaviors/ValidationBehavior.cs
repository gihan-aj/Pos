using System.Reflection;
using FluentValidation;
using MediatR;
using Pos.Web.Shared.Abstractions;
using Pos.Web.Shared.Errors;

namespace Pos.Web.Infrastructure.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse> where TRequest : class
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (!failures.Any())
            {
                return await next();
            }

            // Map to Error
            var errors = failures
                .Select(f => Error.Validation(f.PropertyName, f.ErrorMessage))
                .ToArray();

            if(typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)ValidationResult.WithErrors(errors);
            }

            if(typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0]; // Get 'Guid' from Result<Guid>
                var validationResultType = typeof(ValidationResult<>).MakeGenericType(resultType);

                var validationResult = Activator.CreateInstance(
                    validationResultType,
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    new object[] { errors },
                    null);

                return (TResponse)validationResult!;
            }

            throw new InvalidOperationException("Command/Query must return Result or Result<T>");
        }
    }
}
