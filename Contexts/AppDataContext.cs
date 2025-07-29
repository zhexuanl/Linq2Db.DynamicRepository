using LinqToDB;
using LinqToDB.Data;

public class AppDataContext : DataConnection
{
    public AppDataContext(string configuration) : base(configuration) { }

    public ITable<Customer> Customers => GetTable<Customer>();
}
