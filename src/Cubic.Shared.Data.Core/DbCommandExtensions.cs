using Cubic.Shared.Data.Core.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public static class DbCommandExtensions
  {
    public static DataTable GetDataTable(this DbCommand command, string tablename = null)
    {
      var cmd = command ?? throw new ArgumentNullException(nameof(command));
      var provider = DbConnectionExtensions.GetProvider(cmd.Connection);

      var adapter = provider?.CreateDataAdapter();

      if(adapter != null)
      {
        adapter.SelectCommand = command;
        var dt = string.IsNullOrEmpty(tablename) ? new DataTable() : new DataTable(tablename);
        adapter.Fill(dt);

        return dt;
      }

      return null;
    }

    public static void AddParameters(this DbCommand command, DbInformation dbInformation = null, params KeyValuePair<string, object>[] parameters)
    {
      if (parameters != null)
      {
        foreach (var value in parameters)
        {
          if (!(value is DbParameter))
          {
            var dbInfo = dbInformation ?? DbInformation.Create(command.Connection);
            var name = dbInfo.GetParameterExpression(value.Key, true);
            var parameter = command.CreateParameter();
            parameter.Direction = ParameterDirection.Input;
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
          }
          else
          {
            command.Parameters.Add(value);
          }
        }
      }
    }

    public static string GetDynamicSql(this DbCommand command, CultureInfo culture = null, params object[] parametervalues)
    {
      if (command is null)
      {
        throw new ArgumentNullException(nameof(command));
      }

      var builder = new StringBuilder(command.CommandText);
      var parameters = command.Parameters;

      var useOtherParameterValues = parametervalues != null;
      var formatprovider = culture ?? CultureInfo.InvariantCulture;

      for (int i = 0; i < parameters.Count; i++)
      {
        var name = parameters[i].ParameterName;
        var usedValue = useOtherParameterValues ? parametervalues[i] : parameters[i].Value;

        var convertiable = usedValue as IConvertible;

        if (convertiable != null)
        {
          builder.Replace(name, convertiable.ToString(formatprovider));
        }
        else
        {
          builder.Replace(name, usedValue.ToString());
        }
      }

      return builder.ToString();


    }

    public static int GetHashCode(DbCommand command, bool withParameters)
    {
      var hash = command.CommandText.GetHashCode();

      if(withParameters && command.Parameters?.Count > 0)
      {
        foreach (DbParameter item in command.Parameters)
        {
          hash ^= item.Value.GetHashCode();
        }
      }

      return hash;
    }
  }
}
