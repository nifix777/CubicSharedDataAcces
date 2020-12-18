using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public class ReadUncommitedCommand : DbCommand, IDisposable
  {
    private readonly DbCommand _command;

    private readonly DbTransaction _trx;

    public ReadUncommitedCommand(DbConnection connection) : this(connection.CreateCommand(), connection.BeginTransaction(IsolationLevel.ReadCommitted))
    {

    }
    public ReadUncommitedCommand(DbCommand command, DbTransaction trx)
    {
      _command = command ?? throw new ArgumentNullException(nameof(command));
      _trx = trx ?? throw new ArgumentNullException(nameof(trx));
    }

    public override string CommandText { get => _command.CommandText; set => _command.CommandText = value; }
    public override int CommandTimeout { get => _command.CommandTimeout; set => _command.CommandTimeout = value; }
    public override CommandType CommandType { get => _command.CommandType; set => _command.CommandType = value; }
    public override bool DesignTimeVisible { get => _command.DesignTimeVisible; set => _command.DesignTimeVisible = value; }
    public override UpdateRowSource UpdatedRowSource { get => _command.UpdatedRowSource; set => _command.UpdatedRowSource = value; }
    protected override DbConnection DbConnection { get => _command.Connection; set => _command.Connection = value; }

    protected override DbParameterCollection DbParameterCollection => _command.Parameters;

    protected override DbTransaction DbTransaction { get => _command.Transaction; set => _command.Transaction = value; }

    public override void Cancel()
    {
      _command.Cancel();
    }

    public override int ExecuteNonQuery()
    {
      return _command.ExecuteNonQuery();
    }

    public override object ExecuteScalar()
    {
      return _command.ExecuteScalar();
    }

    public override void Prepare()
    {
      _command.Prepare();
    }

    protected override DbParameter CreateDbParameter()
    {
      return _command.CreateParameter();
    }

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
      return _command.ExecuteReader(behavior);
    }

    protected override void Dispose(bool disposing)
    {
      _trx?.Dispose();
      base.Dispose(disposing);
    }
  }
}
