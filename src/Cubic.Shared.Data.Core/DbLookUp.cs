using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public class DbLookUp : IDbLookUp
  {
    private readonly DbConnection dbConnection;

    public DbLookUp(DbConnection dbConnection)
    {
      this.dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
    }

    public bool DataExists(string column, string source, string clause, IEnumerable<KeyValuePair<string, object>> parameters = null)
    {
      using (var command = dbConnection.CreateCommand())
      {
        command.CommandText = $"SELECT {column} FROM {source} WHERE {clause}";

        if (parameters != null)
        {
          command.Parameters.Clear();

          foreach (var item in parameters)
          {
            command.AddInParameter(item.Key, item.Value);
          }
        }

        return command.ExecuteScalar() != null;
      }
    }

    public T DataExists<T>(string column, string source, string clause, IEnumerable<KeyValuePair<string, object>> parameters = null)
    {
      using (var command = dbConnection.CreateCommand())
      {
        command.CommandText = $"SELECT {column} FROM {source} WHERE {clause}";

        if (parameters != null)
        {
          command.Parameters.Clear();

          foreach (var item in parameters)
          {
            command.AddInParameter(item.Key, item.Value);
          }
        }

        if (TryGetResult(command, out object result)) return (T)result;

        return (T)command.ExecuteScalar();
      }
    }

    public object GetValue(string column, string source, string clause, IEnumerable<KeyValuePair<string, object>> parameters = null)
    {
      using (var command = dbConnection.CreateCommand())
      {
        command.CommandText = $"SELECT {column} FROM {source} WHERE {clause}";

        if (parameters != null)
        {
          command.Parameters.Clear();

          foreach (var item in parameters)
          {
            command.AddInParameter(item.Key, item.Value);
          }
        }

        if (TryGetResult(command, out object result)) return result;

        return command.ExecuteScalar();
      }
    }

    public T GetValue<T>(string column, string source, string clause, IEnumerable<KeyValuePair<string, object>> parameters = null)
    {
      return (T)GetValue(column, source, clause, parameters);
    }

    public IDictionary<string, object> GetData(IEnumerable<string> columns, string source, string clause, IEnumerable<KeyValuePair<string, object>> parameters = null)
    {
      using (var command = dbConnection.CreateCommand())
      {
        command.CommandText = $"SELECT {string.Join(",", columns)} FROM {source} WHERE {clause}";

        if (parameters != null)
        {
          command.Parameters.Clear();

          foreach (var item in parameters)
          {
            command.AddInParameter(item.Key, item.Value);
          }
        }

        if (TryGetResult(command, out object result)) return (IDictionary<string, object>)result;

        using (var reader = command.ExecuteReader())
        {
          if (reader.Read())
          {
            return reader.GetKeyValues();
          }
        }
      }

      return null;
    }

    protected virtual bool TryGetResult(DbCommand dbCommand, out object result)
    {
      result = null;
      return false;
    }

  }
}
