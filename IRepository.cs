using System.Linq.Expressions;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> QueryAsync(QueryOptions<T> options);
}
