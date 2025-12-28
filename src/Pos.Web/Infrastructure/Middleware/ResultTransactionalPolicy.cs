using JasperFx;
using JasperFx.CodeGeneration;
using JasperFx.CodeGeneration.Frames;
using Pos.Web.Infrastructure.Persistence;
using Pos.Web.Shared.Abstractions;
using Wolverine.Configuration;
using Wolverine.Http;
using Wolverine.Runtime.Handlers;

namespace Pos.Web.Infrastructure.Middleware
{
    public class ResultTransactionalPolicy : IHandlerPolicy
    {
        public void Apply(IReadOnlyList<HandlerChain> chains, GenerationRules rules, IServiceContainer container)
        {
            foreach(var chain in chains)
            {
                // Apply only if the handler method actually uses AppDbContext
                // This prevents opening DB connections for unrelated handlers.
                // We check if the Handler class has AppDbContext injected or the method uses it.
                if (chain.HandlerCalls().Any(call => call.UsesType(typeof(AppDbContext))))
                {
                    chain.Middleware.Add(new MethodCall(typeof(ResultTransactionalMiddleware), nameof(ResultTransactionalMiddleware.InvokeAsync)));
                }
            }
        }
    }

    internal static class HandlerCallExtensions
    {
        public static bool UsesType(this MethodCall call, Type type)
        {
            // Checks parameters or dependencies for the type
            return call.Method.GetParameters().Any(p => p.ParameterType == type) ||
                   call.HandlerType.GetConstructors().Any(c => c.GetParameters().Any(p => p.ParameterType == type));
        }
    }
}
