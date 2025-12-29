using MediatR;

namespace Pos.Web.Shared.Abstractions
{
    public interface ICommand : IRequest<Result> { }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
}
