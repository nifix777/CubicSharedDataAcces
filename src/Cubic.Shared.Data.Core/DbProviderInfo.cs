using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  #if NETSTANDARD2_1
  public class DbProviderInfo
  {
    public DbProviderInfo(DataRow prov)
    {
      Name = (string)prov["Name"];
      Description = (string)prov["Description"];
      InvariantName = (string)prov["InvariantName"];
      AssemblyQualifiedName = (string)prov["AssemblyQualifiedName"];

      Factory = System.Data.Common.DbProviderFactories.GetFactory(prov);
    }

    public DbProviderInfo(string name, string description, string invariantName, string assemblyQualifiedName,
        DbProviderFactory factory)
    {
      Name = name;
      Description = description;
      InvariantName = invariantName;
      AssemblyQualifiedName = assemblyQualifiedName;
      Factory = factory;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public string InvariantName { get; set; }
    public string AssemblyQualifiedName { get; set; }
    public Type BulkInsertType { get; set; }
    public DbProviderFactory Factory { get; private set; }
    public String ConnectionString { get; set; }
  }

#endif
}
