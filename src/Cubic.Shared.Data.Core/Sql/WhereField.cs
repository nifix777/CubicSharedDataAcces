using System;
using System.Text;

namespace Cubic.Shared.Data.Core.Sql
{
  [Serializable]
  public class WhereField : Where
  {
    public string Left { get; set; }

    public string Right { get; set; }
  }
}
