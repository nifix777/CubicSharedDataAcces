using System;
using System.Collections.Generic;
using System.Text;

namespace Cubic.Shared.Data.Core.Sql
{
  public class Join
  {
    public SqlAliasable Left { get; }

    public SqlAliasable Right { get; }
  }
}
