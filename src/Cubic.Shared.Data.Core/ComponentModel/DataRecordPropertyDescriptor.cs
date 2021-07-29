using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Cubic.Shared.Data.Core.ComponentModel
{
  public class DataRecordPropertyDescriptor : System.ComponentModel.PropertyDescriptor
  {
    private readonly string propertyName;
    private readonly Type propertyType;

    public DataRecordPropertyDescriptor(string propertyName, Type propertyType, Attribute[] attributes) : base(propertyName, attributes)
    {
      this.propertyName = propertyName;
      this.propertyType = propertyType;
    }

    public override Type ComponentType => typeof(IDataRecord);

    public override bool IsReadOnly => true;

    public override Type PropertyType => propertyType;

    public override bool CanResetValue(object component)
    {
      return false;
    }

    public override object GetValue(object component)
    {
      return ((IDataRecord)component).GetValue(propertyName);
    }

    public override void ResetValue(object component)
    {

    }

    public override void SetValue(object component, object value)
    {
      throw new InvalidOperationException();
    }

    public override bool ShouldSerializeValue(object component)
    {
      return false;
    }
  }
}
