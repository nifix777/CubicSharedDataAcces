using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public class ReadUncommitedConnection : DbConnection, IDisposable
  {
    private readonly DbConnection _conn;

    public ReadUncommitedConnection(DbConnection conn)
    {
      _conn = conn ?? throw new ArgumentNullException(nameof(conn));
    }

    public override string ConnectionString { get => _conn.ConnectionString; set => _conn.ConnectionString = value; }

    public override string Database => _conn.Database;

    public override string DataSource => _conn.DataSource;

    public override string ServerVersion => _conn.ServerVersion;

    public override ConnectionState State => _conn.State;

    public override void ChangeDatabase(string databaseName)
    {
      _conn.ChangeDatabase(databaseName);
    }

    public override void Close()
    {
      _conn.Close();
    }

    public override void Open()
    {
      _conn.Open();
    }

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
      return _conn.BeginTransaction(IsolationLevel.ReadUncommitted);
    }

    protected override DbCommand CreateDbCommand()
    {
      var command = _conn.CreateCommand();
      command.Transaction = BeginTransaction(IsolationLevel.ReadUncommitted);
      return command;
    }


  }
}
