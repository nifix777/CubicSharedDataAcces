using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public static class Materialization
  {
    public static IEnumerable<TItem> Materialize<TItem>(IDataReader reader, Func<TItem> factory)
    {
      var props = TypeDescriptor.GetProperties(typeof(TItem));

      while (reader.Read())
      {
        var component = factory();
        Materialize(reader, component, props);
        yield return component;
      }
    }

    public static void Materialize(IDataRecord record, object component, PropertyDescriptorCollection properties)
    {
      foreach (PropertyDescriptor property in properties)
      {
        var index = record.GetOrdinal(property.Name.ToLowerInvariant());

        if(index >= 0)
        {
          var targetType = property.PropertyType;
          var sourceType = record.GetFieldType(index);
          var value = record.GetValue(index);

          if(targetType == sourceType || targetType.IsAssignableFrom(sourceType))
          {
            property.SetValue(component, value);
          }
          else
          {
            property.SetValue(component, Convert.ChangeType(value, targetType));
          }
        }
      }
    }
  }
}
