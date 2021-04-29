using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cubic.Shared.Data.Core
{
  public static class DbConnectionExtensions
  {

    private static PropertyInfo DbProviderFactoryMethod = typeof(DbConnection).GetProperty("DbProviderFactory", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

    internal static DbProviderFactory GetProvider(DbConnection connection)
    {
      return (DbProviderFactory)DbProviderFactoryMethod.GetValue(connection);
    }

  }
}
