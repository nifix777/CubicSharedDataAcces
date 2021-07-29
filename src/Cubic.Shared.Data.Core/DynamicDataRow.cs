using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public class DynamicDataRow : DynamicObject
  {
    private readonly DataRow dataRow;

    public DynamicDataRow(DataRow dataRow)
    {
      this.dataRow = dataRow ?? throw new ArgumentNullException(nameof(dataRow));
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      try
      {
        result = dataRow[binder.Name];

        if (result == DBNull.Value)
          result = null;

        return true;
      }
      catch { }

      result = null;
      return false;
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
      try
      {
        if (value == null)
          value = DBNull.Value;

        dataRow[binder.Name] = value;
        return true;
      }
      catch { }

      return false;
    }
  }
}
