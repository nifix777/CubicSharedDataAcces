using System;
using System.Data.Common;

namespace Cubic.Shared.Data.Core
{
  public class DbDataSourceBase : DbDataSource
  {
    private readonly DbProviderFactory _providerFactory;
    private readonly string _connectionstring;

    public DbDataSourceBase(DbProviderFactory providerFactory, string connectionstring)
    {
      _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
      _connectionstring = connectionstring ?? throw new ArgumentNullException(nameof(connectionstring));
    }

    public override string ConnectionString => _connectionstring;

    protected override DbConnection GetDbConnection()
    {
      var conn = _providerFactory.CreateConnection();
      conn.ConnectionString = _connectionstring;
      return conn;
    }
  }
}