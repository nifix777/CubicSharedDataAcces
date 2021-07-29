using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public static class Utils
  {
    public static DateTime MinimumSqlDate = new DateTime(1900, 1, 1);

#if NETFULL
    public static string RetrieveConnectionStringFromConfig(string connectionStringName, DbProviderFactory factory )
		{
			// it's a connection string entry
			var connInfo = ConfigurationManager.ConnectionStrings[connectionStringName];
			if (connInfo != null)
			{
				if (!string.IsNullOrEmpty(connInfo.ProviderName))
					factory = DbProviderFactories.GetFactory(connInfo.ProviderName);

				connectionStringName = connInfo.ConnectionString;
			}
			else
				throw new InvalidOperationException("Invalid ConnectionString Name"+ ": " + connectionStringName);
			return connectionStringName;
		}
#endif

    public static IEnumerable<T> Map<T>(this DbDataReader reader)
    {
      if (reader.HasRows)
      {
        var properties = TypeDescriptor.GetProperties(typeof(T));

        while (reader.Read())
        {
          var instance = Activator.CreateInstance<T>();
          foreach (PropertyDescriptor prop in properties)
          {
            if (!prop.IsReadOnly)
            {
              var ordinal = reader.GetOrdinal(prop.Name.ToLower());
              if (ordinal != -1)
              {
                var value = reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal);

                var converter = prop.Converter;

                if (converter != null && converter.CanConvertFrom(reader.GetFieldType(ordinal)))
                {
                  value = converter.ConvertFrom(value);
                }
                prop.SetValue(instance, value);

              }
            }
          }

          yield return instance;
        }
      }
    }

    public static void ConvertToProperty(PropertyDescriptor propertyDescriptor, object instance, object value, Type valueType)
    {
      var converter = propertyDescriptor.Converter;

      var sourceType = valueType ?? value.GetType();

      if (converter != null && converter.CanConvertFrom(sourceType))
      {
        value = converter.ConvertFrom(value);
      }
      propertyDescriptor.SetValue(instance, value);
    }

    public static object ConvertFromProperty(PropertyDescriptor propertyDescriptor, object instance, Type targetType)
    {
      var converter = propertyDescriptor.Converter;

      if (converter != null && converter.CanConvertTo(targetType))
      {
        return converter.ConvertTo(propertyDescriptor.GetValue(instance), targetType);
      }
      return propertyDescriptor.GetValue(instance);
    }

  }
}
