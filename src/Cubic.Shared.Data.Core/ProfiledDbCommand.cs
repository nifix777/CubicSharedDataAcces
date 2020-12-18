using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public sealed class ProfiledDbCommand : DbCommand
  {
    private readonly DbCommand _command;

    private readonly bool _catchExcpetions;

    public ProfiledDbCommand(DbCommand command, bool catchExcpetions)
    {
      _command = command ?? throw new ArgumentNullException(nameof(command));
      _catchExcpetions = catchExcpetions;
    }

    public EventHandler<CommandTimeEventArgs> OnDuration { get; set; }
    public EventHandler<CommandExceptionEventArgs> OnError { get; set; }

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
      if(_catchExcpetions)
      {
        return ProfileAndCatch(_command.ExecuteNonQuery);
      }

      return Profile(_command.ExecuteNonQuery);

    }

    public override object ExecuteScalar()
    {
      if (_catchExcpetions)
      {
        return ProfileAndCatch(_command.ExecuteScalar);
      }

      return Profile(_command.ExecuteScalar);
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
      if (_catchExcpetions)
      {
        return ProfileAndCatch(() => _command.ExecuteReader(behavior));
      }

      return Profile(() => _command.ExecuteReader(behavior));

    }

    private TResult ProfileAndCatch<TResult>(Func<TResult> func)
    {
      var sw = Cubic.Core.Diagnostics.ValueStopwatch.StartNew();
      try
      {
        var result = func();

        var profile = OnDuration;
        profile?.Invoke(this, new CommandTimeEventArgs(sw.GetElapsedTime(), Sql.SqlHelper.CommandAsText(_command)));
        return result;
      }
      catch (Exception ex)
      {
        var onError = OnError;
        onError?.Invoke(this, new CommandExceptionEventArgs(ex, Sql.SqlHelper.CommandAsText(_command)));
        
        throw;
      }
    }

    private TResult Profile<TResult>(Func<TResult> func)
    {
      var sw = Cubic.Core.Diagnostics.ValueStopwatch.StartNew();
      var result = func();

      var profile = OnDuration;
      profile?.Invoke(this, new CommandTimeEventArgs(sw.GetElapsedTime(), Sql.SqlHelper.CommandAsText(_command)));
      return result;
    }
  }

  public class CommandTimeEventArgs : EventArgs
  {
    public CommandTimeEventArgs(TimeSpan duration, string commandText)
    {
      Duration = duration;
      CommandText = commandText ?? throw new ArgumentNullException(nameof(commandText));
    }

    public TimeSpan Duration { get; private set; }

    public string CommandText { get; private set; }
  }

  public class CommandExceptionEventArgs : EventArgs
  {
    public CommandExceptionEventArgs(Exception exception, string commandText)
    {
      Exception = exception ?? throw new ArgumentNullException(nameof(exception));
      CommandText = commandText ?? throw new ArgumentNullException(nameof(commandText));
    }

    public Exception Exception { get; private set; }

    public string CommandText { get; private set; }
  }

}
