using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Cubic.Shared.Data.Core
{
  public static class DbConnectionExtensions
  {

    private static PropertyInfo DbProviderFactoryMethod = typeof(DbConnection).GetProperty("DbProviderFactory", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

    internal static DbProviderFactory GetProvider(this DbConnection connection)
    {
      return (DbProviderFactory)DbProviderFactoryMethod.GetValue(connection);
    }

    public static IEnumerable<T> Query<T>(this DbCommand queryCommand)
    {
      using (var reader = queryCommand.ExecuteReader())
      {
        return Utils.Map<T>(reader);
      }
    }

    public static DbCommand CreateDbCommand(this DbConnection connection, CommandType commandType = CommandType.Text)
    {
      var cmd = connection.CreateCommand();
      cmd.CommandType = commandType;
      return cmd;
    }

    public static IEnumerable<T> Query<T>(this DbConnection connection, string query, params object[] parameters)
    {
      using (var cmd = connection.CreateDbCommand())
      {
        cmd.CommandText = query;

        cmd.AddParameters(parameters);

        return cmd.Query<T>();
      }
    }

    public static int ChangeData<T>(T entity, DbCommand selectCommand, bool updateEntitiy)
    {
      if (selectCommand is null)
      {
        throw new ArgumentNullException(nameof(selectCommand));
      }

      using (var adapter = selectCommand.Connection?.GetProvider()?.CreateDataAdapter())
      {
        if (adapter == null) throw new InvalidOperationException();

        adapter.SelectCommand = selectCommand;

        using (var builder = selectCommand.Connection?.GetProvider()?.CreateCommandBuilder())
        {
          builder.DataAdapter = adapter;

          adapter.UpdateCommand = builder.GetUpdateCommand();

          var table = new DataTable();
          adapter.Fill(table);

          var row = updateEntitiy ? table.Rows[0] : table.NewRow();

          foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(typeof(T)))
          {
            var columnName = property.Name.ToLower();
            var column = table.Columns[columnName];
            row[column] = Utils.ConvertFromProperty(property, entity, column.DataType);
          }

          if (!updateEntitiy) table.Rows.Add(row);

          return adapter.Update(table);
        }
      }


    }
  }
}
