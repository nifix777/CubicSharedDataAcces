using System;
using System.Collections.Generic;

namespace Cubic.Shared.Data.Core.Sql
{
  [Serializable]
  public class WhereIn : Where
  {
    public IEnumerable<object> Values = new List<object>();
  }
}
