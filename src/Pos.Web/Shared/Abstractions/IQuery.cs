using MediatR;

namespace Pos.Web.Shared.Abstractions
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
}
