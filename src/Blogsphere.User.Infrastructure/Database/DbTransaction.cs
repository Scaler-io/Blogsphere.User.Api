using Blogsphere.User.Application.Contracts.Data;

namespace Blogsphere.User.Infrastructure.Database;
public sealed class DbTransaction(UserDbContext dbContext) : IDbTransaction
{
    private readonly UserDbContext _dbContext = dbContext;

    public void BeginTransaction() => _dbContext.Database.BeginTransaction();

    public void CommitTransaction() => _dbContext.Database.CommitTransaction();

    public void RollBackTransaction() => _dbContext.Database.RollbackTransaction();
}
