using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public static class DataRecordExtensions
  {
    public static object GetValue(this IDataRecord record, string name)
    {
      return record.GetValue(record.GetOrdinal(name));
    }
    public static TValue Field<TValue>(this IDataRecord record, string name, TValue defaultValue = default)
    {
      var ordinal = record.GetOrdinal(name);

      var type = record.GetFieldType(ordinal);

      var targetType = typeof(TValue);

      if(targetType.IsAssignableFrom(type))
      {
        return (TValue)record.GetValue(ordinal);
      }

      var isDbNUll = record.IsDBNull(ordinal);

      object value;
      if (typeof(string) == targetType)
      {
        value = Convert.ToString(record.GetValue(ordinal));
        return (TValue)value;
      }
      else if(isDbNUll)
      {
        value = defaultValue;
      }
      else
      {
        value = Convert.ChangeType(record.GetValue(ordinal), targetType);
      }

      return (TValue)value;
    }

    public static TValue Field<TValue>(this DataRow row, string name, TValue defaultValue = default)
    {
      var column = row.Table.Columns[name];
      var type = column.DataType;

      var targetType = typeof(TValue);

      if (targetType.IsAssignableFrom(type))
      {
        return (TValue)row[column];
      }

      var isDbNUll = row.IsNull(column);

      object value;
      if (typeof(string) == targetType)
      {
        value = Convert.ToString(row[column]);
        return (TValue)value;
      }
      //else if (type.IsValueType || Nullable.GetUnderlyingType(type) == null && isDbNUll)
      //{
      //  value = defaultValue;
      //}
      else if (isDbNUll)
      {
        value = defaultValue;
      }
      else
      {
        value = Convert.ChangeType(row[column], targetType);
      }

      return (TValue)value;
    }
  }
}
