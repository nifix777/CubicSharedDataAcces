using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Cubic.Shared.Data.Core
{
  public abstract class DbBulkCopy : IDisposable
  {
    private readonly bool _ownsConnection;
    private readonly IDbConnection _connection;
    private readonly IDbTransaction _trans;
    private bool disposedValue;

    public DbBulkCopy()
    {
      ColumnMappings = new NameValueCollection();
    }
    public DbBulkCopy(IDbConnection connection) : this()
    {
      _connection = connection;
    }

    public DbBulkCopy(string connectionstring) : this()
    {
      _connection = CreateConnection(connectionstring);
      _ownsConnection = true;
    }

    public abstract IDbConnection CreateConnection(string connectionstring);
    public abstract void WriteToServer(DataTable table);
    public abstract void WriteToServer(DataTable table, DataRowState rowState);
    public abstract void WriteToServer(IDataReader reaader);
    public abstract void WriteToServer(DbDataReader reaader);
    public abstract void WriteToServer(IEnumerable<DataRow> rows);

    public abstract Task WriteToServerAsync(DataTable table);
    public abstract void WriteToServerAsync(DataTable table, DataRowState rowState);
    public abstract void WriteToServerAsync(IDataReader reaader);
    public abstract void WriteToServerAsync(DbDataReader reaader);
    public abstract void WriteToServerAsync(IEnumerable<DataRow> rows);
    public NameValueCollection ColumnMappings { get; set; }

    public int NotifyAfter { get; set; }

    public EventHandler RowsWriten;

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
          if(_ownsConnection && _connection != null)
          {
            _connection?.Dispose();
          }
          if (_ownsConnection && _trans != null)
          {
            _trans?.Dispose();
          }
        }

        // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
        // TODO: Große Felder auf NULL setzen
        disposedValue = true;
      }
    }

    // // TODO: Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
    // ~DbBulkCopy()
    // {
    //     // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
      // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }
  }
}
