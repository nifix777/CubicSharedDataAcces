using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public class DataSourceInstance
  {

    public DataSourceInstance(string servername, string instancename, bool? clusterd, string version, string factoryname)
    {
      Servername = servername ?? throw new ArgumentNullException(nameof(servername));
      Instancename = instancename;
      Clusterd = clusterd;
      Version = version;
      Factoryname = factoryname;
    }

    public string Servername { get; }
    public string Instancename { get; }
    public bool? Clusterd { get; }
    public string Version { get; }
    public string Factoryname { get; }
  }
}
