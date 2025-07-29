public interface IQueryFilter<TEntity>
{
    Expression<Func<TEntity, bool>>? Predicate { get; }
    List<OrderDefinition<TEntity>> OrderBy { get; }
    Expression<Func<TEntity, object>>? GroupBy { get; }
}


public interface IPageQuery
{
    int Page { get; }
    int PageSize { get; }
}

public class OrderDefinition<TEntity>
{
    public Expression<Func<TEntity, object>> Property { get; set; } = default!;
    public bool Descending { get; set; }
}