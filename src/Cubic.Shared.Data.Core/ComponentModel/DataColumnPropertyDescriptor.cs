using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Cubic.Shared.Data.Core.ComponentModel
{
  public class DataColumnPropertyDescriptor : System.ComponentModel.PropertyDescriptor
  {
    private readonly System.Data.DataColumn column;

    public DataColumnPropertyDescriptor(DataColumn column, Attribute[] attributes) : base(column.ColumnName, attributes)
    {
      this.column = column;
    }

    public override Type ComponentType => typeof(DataColumn);

    public override bool IsReadOnly => false;

    public override Type PropertyType => column.DataType;

    public override bool CanResetValue(object component)
    {
      return false;
    }

    public override object GetValue(object component)
    {
      return ((DataRow)component)[column];
    }

    public override void ResetValue(object component)
    {
      
    }

    public override void SetValue(object component, object value)
    {
      ((DataRow)component)[column] = value;
    }

    public override bool ShouldSerializeValue(object component)
    {
      return false;
    }
  }
}
