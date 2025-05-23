using MediatR;

namespace Blogsphere.User.Application.Contracts.CQRS;
public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
{
}
