using LinqToDB;
using LinqToDB.Data;
using SqlKata;
using SqlKata.Compilers;
using System.Linq.Expressions;

public class Linq2DbRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly IDataContext _db;
    protected readonly ITable<TEntity> _table;

    public Linq2DbRepository(IDataContext db)
    {
        _db = db;
        _table = db.GetTable<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(object id)
        => await _table.FindAsync(id);

    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await _table.ToListAsync();

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        => await _table.Where(predicate).ToListAsync();

    public async Task<PagedResult<TEntity>> QueryAsync(IQueryFilter<TEntity> filter, IPageQuery? paging = null)
    {
        var query = _table.AsQueryable();

        if (filter.Predicate != null)
            query = query.Where(filter.Predicate);

        foreach (var order in filter.OrderBy)
            query = order.Descending
                ? query.OrderByDescending(order.Property)
                : query.OrderBy(order.Property);

        if (filter.GroupBy != null)
            query = query.GroupBy(filter.GroupBy).Select(g => g.First());

        var count = await query.CountAsync();

        if (paging != null)
            query = query.Skip((paging.Page - 1) * paging.PageSize).Take(paging.PageSize);

        var result = await query.ToListAsync();
        return new PagedResult<TEntity>(result, count, paging?.Page ?? 1, paging?.PageSize ?? count);
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate = null)
        => predicate == null ? _table.CountAsync() : _table.CountAsync(predicate);

    public Task InsertAsync(TEntity entity) => _db.InsertAsync(entity);
    public Task InsertBulkAsync(IEnumerable<TEntity> entities) => _db.BulkCopyAsync(entities);
    public Task UpdateAsync(TEntity entity) => _db.UpdateAsync(entity);
    public Task DeleteAsync(TEntity entity) => _db.DeleteAsync(entity);

    public async Task<IEnumerable<TEntity>> FromSqlKataAsync(Query query)
    {
        var compiler = new SqlServerCompiler(); // Replace with OracleCompiler if needed
        var compiled = compiler.Compile(query);

        var sql = compiled.Sql;
        var bindings = compiled.Bindings.ToArray();

        return await _db.Query<TEntity>().FromSql(sql, bindings).ToListAsync();
    }
}
