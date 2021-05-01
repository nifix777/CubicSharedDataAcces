using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public abstract class DbUpdateableDataRecordSet : DbDataReader, IUpdatableDataRecord
  {
    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value"></param>
    public void SetBoolean(int ordinal, bool value)
    {
      SetValue(ordinal, value);
    }
    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value"></param>
    public void SetByte(int ordinal, byte value)
    {
      SetValue(ordinal, value);
    }
    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value">The new field value</param>
    public void SetChar(int ordinal, char value)
    {
      SetValue(ordinal, value);
    }

    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value">The new field value</param>
    public void SetDataRecord(int ordinal, IDataRecord value)
    {
      SetValue(ordinal, value);
    }
    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value">The new field value</param>
    public void SetDateTime(int ordinal, DateTime value)
    {
      SetValue(ordinal, value);
    }
    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value">The new field value</param>
    public void SetDecimal(int ordinal, Decimal value)
    {
      SetValue(ordinal, value);
    }
    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value">The new field value</param>
    public void SetDouble(int ordinal, Double value)
    {
      SetValue(ordinal, value);
    }
    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value">The new field value</param>
    public void SetFloat(int ordinal, float value)
    {
      SetValue(ordinal, value);
    }
    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value">The new field value</param>
    public void SetGuid(int ordinal, Guid value)
    {
      SetValue(ordinal, value);
    }
    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value">The new field value</param>
    public void SetInt16(int ordinal, Int16 value)
    {
      SetValue(ordinal, value);
    }
    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value">The new field value</param>
    public void SetInt32(int ordinal, Int32 value)
    {
      SetValue(ordinal, value);
    }
    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value">The new field value</param>
    public void SetInt64(int ordinal, Int64 value)
    {
      SetValue(ordinal, value);
    }
    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value">The new field value</param>
    public void SetString(int ordinal, string value)
    {
      SetValue(ordinal, value);
    }
    /// <summary>
    /// Sets the value of a field in a record
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    /// <param name="value">The new field value</param>
    public void SetValue(int ordinal, object value)
    {
      SetRecordValue(ordinal, value);
    }
    /// <summary>
    /// Sets field values in a record
    /// </summary>
    /// <param name="values"></param>
    /// <returns>The number of fields that were set</returns>
    public int SetValues(params object[] values)
    {
      int minValue = Math.Min(values.Length, FieldCount);
      for (int i = 0; i < minValue; i++)
      {
        SetRecordValue(i, values[i]);
      }
      return minValue;
    }
    /// <summary>
    /// Sets a field to the DBNull value
    /// </summary>
    /// <param name="ordinal">The ordinal of the field</param>
    public void SetDBNull(int ordinal)
    {
      SetRecordValue(ordinal, DBNull.Value);
    }

    protected abstract void SetRecordValue(int ordinal, object value);
  }
}
