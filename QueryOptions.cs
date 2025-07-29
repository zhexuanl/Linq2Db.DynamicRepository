public class QueryWrapper<TEntity> where TEntity : class
{
    public Expression<Func<TEntity, bool>>? Predicate { get; private set; }
    public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? OrderBy { get; private set; }
    public int? Skip { get; private set; }
    public int? Take { get; private set; }

    public QueryWrapper<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        Predicate = Predicate == null ? predicate : Predicate.AndAlso(predicate);
        return this;
    }

    public QueryWrapper<TEntity> OrderByAsc<TKey>(Expression<Func<TEntity, TKey>> keySelector)
    {
        OrderBy = q => q.OrderBy(keySelector);
        return this;
    }

    public QueryWrapper<TEntity> OrderByDesc<TKey>(Expression<Func<TEntity, TKey>> keySelector)
    {
        OrderBy = q => q.OrderByDescending(keySelector);
        return this;
    }

    public QueryWrapper<TEntity> Page(int pageIndex, int pageSize)
    {
        Skip = pageIndex * pageSize;
        Take = pageSize;
        return this;
    }
}
