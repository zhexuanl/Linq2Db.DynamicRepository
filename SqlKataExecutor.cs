public class SqlKataExecutor
{
    private readonly IDataContext _db;
    private readonly Compiler _compiler;

    public SqlKataExecutor(IDataContext db, Compiler compiler)
    {
        _db = db;
        _compiler = compiler;
    }

    public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(Query query)
    {
        var sql = _compiler.Compile(query);
        return await _db.QueryToListAsync<T>(sql.Sql, sql.Bindings.ToArray());
    }

    public async Task<int> ExecuteNonQueryAsync(Query query)
    {
        var sql = _compiler.Compile(query);
        return await _db.ExecuteAsync(sql.Sql, sql.Bindings.ToArray());
    }
}
