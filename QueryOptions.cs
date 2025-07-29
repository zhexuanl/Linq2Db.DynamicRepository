using System.Linq.Expressions;

public class QueryOptions<T>
{
    public Expression<Func<T, bool>>? Filter { get; set; }
    public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; set; }
    public Expression<Func<T, object>>? GroupBy { get; set; }
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
}
