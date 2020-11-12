using Cubic.Core.Collections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Cubic.Shared.Data.Core
{
  public class DbStoredProcedure : IDbStoredProcedure
  {
    private readonly string _name;

    private readonly DbConnection _conn;

    public DbStoredProcedure(DbConnection conn, string name)
    {
      _conn = conn ?? throw new ArgumentNullException(nameof(conn));
      _name = name ?? throw new ArgumentNullException(nameof(name));
      Parameters = new List<IDbDataParameter>();
    }

    public string Name => _name;

    public IList<IDbDataParameter> Parameters { get; private set; }

    private DbCommand CreateCommand()
    {
      var cmd = _conn.CreateCommand();
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.CommandText = _name;
      cmd.Parameters.AddRange(Parameters);
      return cmd;
    }

    public DbDataReader Invoke()
    {
      var cmd = CreateCommand();
      return cmd.ExecuteReader();
    }

    public async Task<DbDataReader> InvokeAsync(CancellationToken cancellationToken = default)
    {
      var cmd = CreateCommand();

      return await cmd.ExecuteReaderAsync(cancellationToken);
    }

    public object InvokeScalar()
    {
      var cmd = CreateCommand();

      return cmd.ExecuteScalar();
    }

    public async Task<object> InvokeScalarAsync(CancellationToken cancellationToken = default)
    {
      var cmd = CreateCommand();

      return await cmd.ExecuteScalarAsync(cancellationToken);
    }
  }
}
