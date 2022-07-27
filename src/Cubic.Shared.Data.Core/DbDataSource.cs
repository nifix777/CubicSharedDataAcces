using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cubic.Shared.Data.Core
{
  public abstract class DbDataSource : IDisposable
  {
    public abstract string ConnectionString { get; }

    protected abstract DbConnection GetDbConnection();

    protected virtual DbConnection OpenDbConnection()
    {
      var connection = GetDbConnection();
      connection.Open();

      return connection;
    }

    protected virtual async Task<DbConnection> OpenDbConnectionAsync(CancellationToken cancellationToken = default)
    {
      var connection = GetDbConnection();
      await connection.OpenAsync(cancellationToken);

      return connection;
    }

    protected virtual DbCommand CreateDbCommand(string commandText = null)
    {
      var connection = GetDbConnection();
      var command = connection.CreateCommand();

      return command;
    }

    public DbConnection GetConnection() => GetDbConnection();
    public DbConnection OpenConnection() => OpenDbConnection();
    public Task<DbConnection> OpenConnectionAsync() => OpenDbConnectionAsync();
    public DbCommand CreateCommand(string commandText = null) => CreateDbCommand(commandText);

    public virtual void Dispose()
    {

    }
  }
}
