using System.Linq.Expressions;

namespace Customers.API.Repository;

public interface IMockEntity<TEntity> where TEntity : class
{
    Task CreateAsync(params TEntity[] newEntities);
    Task<IEnumerable<TEntity>> GetAsync();
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression);
    Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> expression);
    Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity updateCustomer);
}