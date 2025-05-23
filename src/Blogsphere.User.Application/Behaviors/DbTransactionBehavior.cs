using Blogsphere.User.Application.Contracts.Data;
using Blogsphere.User.Application.Extensions;
using MediatR;

namespace Blogsphere.User.Application.Behaviors;
public sealed class DbTransactionBehavior<TRequest, TResponse>(IDbTransaction dbTransaction, ILogger logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

    private readonly IDbTransaction _dbTransaction = dbTransaction;
    private readonly ILogger _logger = logger;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _logger.Here().Information("Starting transaction for the request {TRequest}", typeof(TRequest).Name);
        _dbTransaction.BeginTransaction();

        try
        {
            var response = await next();
            _logger.Here().Information("Committing transaction for the request {TRquest}", typeof(TRequest).Name);
            _dbTransaction.CommitTransaction();
            return response;

        }
        catch
        {
            _logger.Here().Information("Rolling back transaction for the request {TRquest}", typeof(TRequest).Name);
            _dbTransaction.RollBackTransaction();
            throw;
        }
    }
}
