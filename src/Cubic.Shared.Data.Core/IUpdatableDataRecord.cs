using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public interface IUpdatableDataRecord : IDataRecord
  {
    void SetBoolean(int ordinal, bool value);

    void SetByte(int ordinal, byte value);

    void SetChar(int ordinal, char value);

    void SetDateTime(int ordinal, DateTime value);

    void SetDecimal(int ordinal, decimal value);

    void SetDouble(int ordinal, double value);

    void SetFloat(int ordinal, float value);

    void SetGuid(int ordinal, Guid value);

    void SetInt16(int ordinal, short value);

    void SetInt32(int ordinal, int value);

    void SetInt64(int ordinal, long value);

    void SetString(int ordinal, string value);

    void SetValue(int ordinal, object value);
    void SetDataRecord(int ordinal, System.Data.IDataRecord value);

    int SetValues(params object[] values);

    void SetDBNull(int ordinal);
  }
}
