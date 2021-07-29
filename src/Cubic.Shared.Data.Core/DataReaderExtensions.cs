using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Cubic.Shared.Data.Core
{
  public static class DataReaderExtensions
  {
    public static IEnumerable<TItem> Read<TItem>(DbDataReader dataReader) where TItem : new()
    {
      if(dataReader.HasRows)
      {
        var properties = TypeDescriptor.GetProperties(typeof(TItem));
        var columns = dataReader.GetColumnSchema();
        while(dataReader.Read())
        {
          var item = new TItem();
          FillObjectFromReader(dataReader, properties, columns, item);

          yield return item;
        }
      }
    }

    private static void FillObjectFromReader<TItem>(IDataReader dataReader, PropertyDescriptorCollection properties, IReadOnlyCollection<DbColumn> columns, TItem item)
    {
      foreach (PropertyDescriptor prop in properties)
      {
        if (!prop.IsReadOnly)
        {
          if (columns.Any(c => string.Equals(c.ColumnName, prop.Name, StringComparison.OrdinalIgnoreCase)))
          {
            var ordinal = dataReader.GetOrdinal(prop.Name);

            //Check TypeConverterAttribute
            if (prop.Converter == null)
            {
              prop.SetValue(item, dataReader.IsDBNull(ordinal) ? null : dataReader.GetValue(ordinal));
            }
            else
            {
              if (prop.Converter.CanConvertFrom(dataReader.GetFieldType(ordinal)))
              {
                prop.SetValue(item, prop.Converter.ConvertFrom(dataReader.GetValue(ordinal)));
              }
            }
          }
          else
          {
            //Check DefaultAttribute
            var @default = prop.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
            if (@default != null)
            {
              prop.SetValue(item, @default.Value);
            }
          }
        }
      }
    }
  }
}
