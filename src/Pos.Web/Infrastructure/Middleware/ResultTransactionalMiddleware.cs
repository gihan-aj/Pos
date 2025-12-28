using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Wolverine;

namespace Pos.Web.Infrastructure.Middleware
{
    public class ResultTransactionalMiddleware
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ResultTransactionalMiddleware> _logger;

        public ResultTransactionalMiddleware(AppDbContext dbContext, ILogger<ResultTransactionalMiddleware> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        // Wolverine automatically injects the "next" delegate as a Func<Task<T>> 
        // where T matches the return type of the Handler (e.g., Result or Result<Guid>)
        public async Task<T> InvokeAsync<T>(Func<Task<T>> next, CancellationToken cancellationToken)
        {
            // 1. Run the Handler
            var handlerResult = await next();

            // 2. Check if the result implements the Result pattern (Result or Result<T>)
            if (handlerResult is Result resultPattern && resultPattern.IsFailure)
            {
                _logger.LogWarning("Transaction rolled back. Handler returned failure result: {Error}", resultPattern.Error);

                // Return the failure immediately. 
                // SaveChangesAsync is SKIPPED, so the DB remains untouched.
                return handlerResult;
            }

            // 3. If Success, Commit Transaction
            // This also flushes any Wolverine Outbox messages found in the context.
            await _dbContext.SaveChangesAsync(cancellationToken);

            return handlerResult;
        }
    }
}
