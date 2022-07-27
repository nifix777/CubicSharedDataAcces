using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public class DbDataSourceBuilderBase : DbDataSourceBuilder
  {
    private readonly DbProviderFactory _factory;

    public DbDataSourceBuilderBase(DbProviderFactory factory)
    {
      _factory = factory;
    }

    public DbDataSourceBuilderBase UseConnectionString(string connectionstring)
    {
      return (DbDataSourceBuilderBase)Configure(dsb => dsb.ConnectionStringBuilder.ConnectionString = connectionstring);
    }

    public override DbDataSource Build()
    {
      return new DbDataSourceBase(_factory, this.ConnectionStringBuilder.ConnectionString);
    }
  }
}
