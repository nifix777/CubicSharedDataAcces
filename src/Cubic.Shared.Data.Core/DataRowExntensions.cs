using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public static class DataRowExntensions
  {
    public static bool CopyTo(this DataRow source, DataRow target)
    {
      var columns = target.Table.Columns;

      for (int x = 0; x < columns.Count; x++)
      {
        string name = columns[x].ColumnName;

        try
        {
          target[x] = source[name];
        }
        catch
        {
          //ignore
        } 
      }

      return true;
    }
  }
}
