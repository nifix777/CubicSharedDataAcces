using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public class Recordset : IDataReader
  {
    private readonly IDataAdapter _dataAdapter;

    private DataSet data;

    private DbDataReader tableReader;
    public Recordset(IDataAdapter dataAdapter)
    {
      _dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
      data = new DataSet();
      data.Tables.Add();
    }

    private DataTable Table => data.Tables[0];

    #region IDataReader

    public object this[int i] => ((IDataRecord)tableReader)[i];

    public object this[string name] => ((IDataRecord)tableReader)[name];

    public int Depth => ((IDataReader)tableReader).Depth;

    public bool IsClosed => ((IDataReader)tableReader).IsClosed;

    public int RecordsAffected => ((IDataReader)tableReader).RecordsAffected;

    public int FieldCount => ((IDataRecord)tableReader).FieldCount;

    public void Close()
    {
      ((IDataReader)tableReader).Close();
    }

    public void Dispose()
    {
      ((IDisposable)tableReader).Dispose();
    }

    public bool GetBoolean(int i)
    {
      return ((IDataRecord)tableReader).GetBoolean(i);
    }

    public byte GetByte(int i)
    {
      return ((IDataRecord)tableReader).GetByte(i);
    }

    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
    {
      return ((IDataRecord)tableReader).GetBytes(i, fieldOffset, buffer, bufferoffset, length);
    }

    public char GetChar(int i)
    {
      return ((IDataRecord)tableReader).GetChar(i);
    }

    public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
    {
      return ((IDataRecord)tableReader).GetChars(i, fieldoffset, buffer, bufferoffset, length);
    }

    public IDataReader GetData(int i)
    {
      return ((IDataRecord)tableReader).GetData(i);
    }

    public string GetDataTypeName(int i)
    {
      return ((IDataRecord)tableReader).GetDataTypeName(i);
    }

    public DateTime GetDateTime(int i)
    {
      return ((IDataRecord)tableReader).GetDateTime(i);
    }

    public decimal GetDecimal(int i)
    {
      return ((IDataRecord)tableReader).GetDecimal(i);
    }

    public double GetDouble(int i)
    {
      return ((IDataRecord)tableReader).GetDouble(i);
    }

    public Type GetFieldType(int i)
    {
      return ((IDataRecord)tableReader).GetFieldType(i);
    }

    public float GetFloat(int i)
    {
      return ((IDataRecord)tableReader).GetFloat(i);
    }

    public Guid GetGuid(int i)
    {
      return ((IDataRecord)tableReader).GetGuid(i);
    }

    public short GetInt16(int i)
    {
      return ((IDataRecord)tableReader).GetInt16(i);
    }

    public int GetInt32(int i)
    {
      return ((IDataRecord)tableReader).GetInt32(i);
    }

    public long GetInt64(int i)
    {
      return ((IDataRecord)tableReader).GetInt64(i);
    }

    public string GetName(int i)
    {
      return ((IDataRecord)tableReader).GetName(i);
    }

    public int GetOrdinal(string name)
    {
      return ((IDataRecord)tableReader).GetOrdinal(name);
    }

    public DataTable GetSchemaTable()
    {
      return ((IDataReader)tableReader).GetSchemaTable();
    }

    public string GetString(int i)
    {
      return ((IDataRecord)tableReader).GetString(i);
    }

    public object GetValue(int i)
    {
      return ((IDataRecord)tableReader).GetValue(i);
    }

    public int GetValues(object[] values)
    {
      return ((IDataRecord)tableReader).GetValues(values);
    }

    public bool IsDBNull(int i)
    {
      return ((IDataRecord)tableReader).IsDBNull(i);
    }

    public bool NextResult()
    {
      return ((IDataReader)tableReader).NextResult();
    }

    public bool Read()
    {
      return ((IDataReader)tableReader).Read();
    }

    #endregion

    public void Fill(IDataReader reader)
    {
      Table.Load(reader);
      tableReader = Table.CreateDataReader();
    }
    public int Update()
    {
      return _dataAdapter.Update(data);
    }
  }
}
