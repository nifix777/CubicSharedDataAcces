using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;

namespace Cubic.Shared.Data.Core
{
  /// <summary>
  /// Represents a manuel DbConnection-Pool
  /// </summary>
  public class DbConnectionPool : IDisposable
  {
    private const int MaxPoolSize = 10;

    private readonly string _connectionString;

    private int _maxConnections;

    private SemaphoreSlim _connectionLock;

    private List<DbConnection> pool;
    private bool disposedValue;

    private readonly Func<string, DbConnection> _connectionFactory;

    /// <summary>
    /// Gets fired when a new Connection is created from the pool.
    /// </summary>
    public event EventHandler<DbConnectionEventArgs> ConnectionCreated;

    /// <summary>
    /// Gets fired when a Connection is released to the pool.
    /// </summary>
    public event EventHandler<EventArgs> ConnectionReleased;

    /// <summary>
    /// Creates a new instance of a ConnectionPool.
    /// </summary>
    /// <param name="connectionString">The connection string that will be used for new connections</param>
    /// <param name="connectionFactory">Method that return a new <see cref="DbConnection"/> for the given <paramref name="connectionString"/> </param>
    /// <param name="maxConnections">Number of maximum hold connections for this pool</param>
    public DbConnectionPool(string connectionString, Func<string, DbConnection> connectionFactory, int maxConnections = MaxPoolSize)
    {
      _connectionString = connectionString;
      _maxConnections = maxConnections;

      _connectionLock = new SemaphoreSlim(maxConnections);
      pool = new List<DbConnection>();
      _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Gets a <see cref="DbConnection"/> from the pool.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ObjectDisposedException"></exception>
    /// <exception cref="OperationCanceledException"></exception>
    public DbConnection GetConnection()
    {
      if (disposedValue)
      {
        throw new ObjectDisposedException(nameof(DbConnectionPool));
      }

      // Single Connection
      if (_maxConnections == 1)
      {
        // Get Unique Open Connection
        if (pool.Count > 0)
          return pool.First();
        else
        {
          // Create Unique Open Connection
          var connection = CreateAndOpenConnection();
          lock (pool)
          {
            pool.Add(connection);

            return connection;
          }
        }
      }
      else
      {
        _connectionLock.Wait(); // Wait until connection pool is released.

        // Double-Check Finalized
        if (disposedValue)
        {
          throw new OperationCanceledException("Connection pool was finalized.");
        }

        // Add Connection to Pool
        var connection = CreateAndOpenConnection();
        lock (pool)
        {
          if (pool.Count <= _maxConnections)
          {
            pool.Add(connection);

            return connection;
          }
          else
          {
            return connection;
          }
        }
      }
    }

    /// <summary>
    /// Release DbConnection from Connection Pool.
    /// </summary>
    public void ReleaseConnection(DbConnection connection)
    {
      if (connection == null) return;
      if (disposedValue)
      {
        CloseAndDisposeConnection(connection);
        return;
      }

      // Single Connection (Not require release)
      if (_maxConnections == 1) return;

      try
      {
        if (_maxConnections > 1) // Double-check not single connection
        {
          // Try Remove Connection
          lock (pool)
          {
            if (pool.Contains(connection))
            {
              // Remove Connection from Pool
              pool.Remove(connection);

              CloseAndDisposeConnection(connection);
            }
          }
        }
      }
      finally
      {
        if (_connectionLock.CurrentCount < _maxConnections) _connectionLock.Release(); // Signal the connection pool to continue.
      }
    }

    /// <summary>
    /// Release All DbConnection from Connection Pool.
    /// </summary>
    public void ReleaseAllConnections()
    {
      // Close & Release All Connections
      lock (pool)
      {
        foreach (var connection in pool)
        {
          CloseAndDisposeConnection(connection);

          if (_connectionLock.CurrentCount < _maxConnections) _connectionLock.Release(); // Signal the connection pool to continue.
        }
        pool.Clear();
      }
    }

    private DbConnection CreateAndOpenConnection()
    {
      // Create and Open Connection
      var conn = _connectionFactory(_connectionString);
      conn.Open();

      ConnectionCreated?.Invoke(this, new DbConnectionEventArgs(conn));

      return conn;
    }

    private void CloseAndDisposeConnection(DbConnection connection)
    {
      // Close and Dispose Connection
      if (connection == null) return;
      if (connection.State != ConnectionState.Closed)
      {
        connection.Close();
      }

      connection.Dispose();

      ConnectionReleased?.Invoke(this, new DbConnectionEventArgs(connection));
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
          ReleaseAllConnections();
        }

        // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
        // TODO: Große Felder auf NULL setzen
        disposedValue = true;
      }
    }

    // // TODO: Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
    // ~DbConnectionPool()
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

    /// <summary>
    /// Represents a <see cref="EventArgs"/> instance for a <see cref="DbConnection"/>
    /// </summary>
    public class DbConnectionEventArgs : EventArgs
    {
      public DbConnectionEventArgs(DbConnection connection)
      {
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));
      }

      public DbConnection Connection { get; private set; }
    }
  }
}
