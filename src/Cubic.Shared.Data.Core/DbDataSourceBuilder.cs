using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public abstract class DbDataSourceBuilder
  {
    public DbConnectionStringBuilder ConnectionStringBuilder { get; }

    protected DbDataSourceBuilder Configure(Action<DbDataSourceBuilder> configurationAction)
    {
      configurationAction?.Invoke(this);
      return this;
    }

    public abstract DbDataSource Build();
  }
}
