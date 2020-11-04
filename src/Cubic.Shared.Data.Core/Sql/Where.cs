using System;
using System.Collections.Generic;
using System.Text;

namespace Cubic.Shared.Data.Core.Sql
{
  [Serializable]
  public class Where
  {
    public bool Invert { get; set; }

    public bool IsAnd { get; set; } = true;

    public string Field { get; set; }

    public object Value { get; set; }
  }
}
