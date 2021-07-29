using Cubic.Core;
using Cubic.Core.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public static class DbParameterExtensions
  {
    private static MethodInfo GetParameterNameMethod = typeof(DbCommandBuilder).GetMethod("GetParameterName", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, new[] { typeof(string) }, null);
    public static string GetParameterName(DbConnection connection, string name)
    {
      var factory = DbConnectionExtensions.GetProvider(connection);
      return (string)GetParameterNameMethod.Invoke(factory.CreateCommandBuilder(), new object[] { name });
    }

    public static IDataParameter CreateParameter(this DbCommand command, string name, object value, Type type, Func<Type, DbType> typeMappingFunc = null)
    {
      var typeMappingFunction = typeMappingFunc ?? DbTypeMapping.Default.GetDbType;
      var parameter = command.CreateParameter();
      //parameter.ParameterName = name;
      parameter.ParameterName = GetParameterName(command.Connection, name);
      parameter.Value = value ?? DBNull.Value;
      parameter.DbType = typeMappingFunction(type);

      //command.Parameters.Add(parameter);

      return parameter;
    }

    public static IDataParameter CreateParameter(this DbCommand command, string name, object value, DbType? type = null)
    {
      var parameter = command.CreateParameter();
      parameter.ParameterName = GetParameterName(command.Connection, name);
      parameter.Value = value ?? DBNull.Value;

      if (type.HasValue)
      {
        parameter.DbType = type.Value;
      }

      //command.Parameters.Add(parameter);

      return parameter;
    }

    public static DbCommand AddInParameter(this DbCommand command, string name, object value, DbType? type = null)
    {
      var cmd = CreateParameter(command, name, value, type);
      cmd.Direction = ParameterDirection.Input;
      command.Parameters.Add(cmd);
      return command;
    }

    public static DbCommand AddInParameter<TValue>(this DbCommand command, string name, TValue value, Func<Type, DbType> typeMappingFunc = null)
    {
      var cmd = CreateParameter(command, name, value, typeof(TValue), typeMappingFunc);
      cmd.Direction = ParameterDirection.Input;
      command.Parameters.Add(cmd);
      return command;
    }

    public static DbCommand AddOutParameter(this DbCommand command, string name, object value, DbType? type = null)
    {
      var cmd = CreateParameter(command, name, value, type);
      cmd.Direction = ParameterDirection.Output;
      command.Parameters.Add(cmd);
      return command;
    }

    public static DbCommand AddOutParameter<TValue>(this DbCommand command, string name, TValue value, Func<Type, DbType> typeMappingFunc = null)
    {
      var cmd = CreateParameter(command, name, value, typeof(TValue), typeMappingFunc);
      cmd.Direction = ParameterDirection.Output;
      command.Parameters.Add(cmd);
      return command;
    }

  }
}
