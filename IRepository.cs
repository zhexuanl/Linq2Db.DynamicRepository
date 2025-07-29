using System.Linq.Expressions;
using SqlKata;
using System.Threading.Tasks;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(object id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<PagedResult<TEntity>> QueryAsync(IQueryFilter<TEntity> filter, IPageQuery? paging = null);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null);

    Task InsertAsync(TEntity entity);
    Task InsertBulkAsync(IEnumerable<TEntity> entities);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);

    Task<IEnumerable<TEntity>> FromSqlKataAsync(Query query);
}
