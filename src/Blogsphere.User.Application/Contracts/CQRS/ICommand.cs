using MediatR;

namespace Blogsphere.User.Application.Contracts.CQRS;
public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<TResponse> : IRequest<TResponse>
    where TResponse : notnull
{

}
