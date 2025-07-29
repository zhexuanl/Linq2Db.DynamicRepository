using LinqToDB;
using LinqToDB.Data;
using System.Linq.Expressions;

public class Linq2DbRepository<T> : IRepository<T> where T : class
{
    protected readonly DataConnection _db;

    public Linq2DbRepository(DataConnection db)
    {
        _db = db;
    }

    public virtual async Task<IEnumerable<T>> QueryAsync(QueryOptions<T> options)
    {
        var query = _db.GetTable<T>().AsQueryable();
        if (options.Filter != null)
            query = query.Where(options.Filter);
        if (options.OrderBy != null)
            query = options.OrderBy(query);
        if (options.GroupBy != null)
            return query.GroupBy(options.GroupBy).SelectMany(g => g).ToList();
        if (options.PageIndex.HasValue && options.PageSize.HasValue)
            query = query.Skip(options.PageIndex.Value * options.PageSize.Value).Take(options.PageSize.Value);
        return await query.ToListAsync();
    }
}
