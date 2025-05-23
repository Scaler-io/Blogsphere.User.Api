namespace Blogsphere.User.Application.Contracts.Data;
public interface IDbTransaction
{
    void BeginTransaction();
    void CommitTransaction();
    void RollBackTransaction();
}
