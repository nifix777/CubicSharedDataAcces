using System;
using System.Collections.Generic;
using System.Text;

namespace Cubic.Shared.Data.Core.Sql
{
  [Serializable]
  public class Sort
  {
    public bool Descending { get; set; }

    public SqlAliasable Field { get; set; }
  }
}
