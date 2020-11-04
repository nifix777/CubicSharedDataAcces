using Cubic.Core;
using Cubic.Core.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public static class DbParameterExtensions
  {
    //private static DbProviderFactory GetProvider<TConnection>(TConnection connection) where TConnection : DbConnection
    //{
    //  return (DbProviderFactory)typeof(TConnection).GetProperty("DbProviderFactory").GetValue(connection);
    //}
    //public static string QuoteIdentifier(this DbConnection connection, string idendtifier)
    //{
    //  return connection.Get
    //}

    public static IDataParameter AddNamedParameter(this IDbCommand command, string name, object value, Type type, Func<Type, DbType> typeMappingFunc = null)
    {
      var typeMappingFunction = typeMappingFunc ?? DbTypeMapping.GetDbType;
      var parameter = command.CreateParameter();
      parameter.ParameterName = name;
      parameter.Value = value ?? DBNull.Value;
      parameter.DbType = typeMappingFunction(type);

      command.Parameters.Add(parameter);

      return parameter;
    }

    public static IDataParameter AddNamedParameter(this IDbCommand command, string name, object value, DbType? type = null)
    {
      var parameter = command.CreateParameter();
      parameter.ParameterName = name;
      parameter.Value = value ?? DBNull.Value;

      if (type.HasValue)
      {
        parameter.DbType = type.Value;
      }

      command.Parameters.Add(parameter);

      return parameter;
    }

    public static IDbCommand With<TValue>(this IDbCommand command, string name, TValue value)
    {
      var parameter = command.CreateParameter();
      parameter.ParameterName = name;
      parameter.Value = value.IsNull() ? DBNull.Value : (object)value;
      command.Parameters.Add(parameter);

      return command;
    }

    public static IDbCommand With(this IDbCommand command, string name, object value)
    {
      var parameter = command.CreateParameter();
      parameter.ParameterName = name;
      parameter.Value = value ?? DBNull.Value;
      command.Parameters.Add(parameter);

      return command;
    }

    public static IDbCommand With(this IDbCommand command, string name, object value, DbType dbType)
    {
      var parameter = command.CreateParameter();
      parameter.ParameterName = name;
      parameter.Value = value ?? DBNull.Value;
      parameter.DbType = dbType;
      command.Parameters.Add(parameter);

      return command;
    }
  }
}
