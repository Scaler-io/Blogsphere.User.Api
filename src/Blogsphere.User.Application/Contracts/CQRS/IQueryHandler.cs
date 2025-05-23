using MediatR;

namespace Blogsphere.User.Application.Contracts.CQRS;
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse: notnull
{
}
