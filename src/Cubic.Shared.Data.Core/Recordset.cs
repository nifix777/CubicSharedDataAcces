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

    private DbDataReader reader;
    public Recordset(IDataAdapter dataAdapter)
    {
      _dataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
      data = new DataSet();
      data.Tables.Add();
    }

    private DataTable Table => data.Tables[0];

    #region IDataReader

    public object this[int i] => ((IDataRecord)reader)[i];

    public object this[string name] => ((IDataRecord)reader)[name];

    public int Depth => ((IDataReader)reader).Depth;

    public bool IsClosed => ((IDataReader)reader).IsClosed;

    public int RecordsAffected => ((IDataReader)reader).RecordsAffected;

    public int FieldCount => ((IDataRecord)reader).FieldCount;

    public void Close()
    {
      ((IDataReader)reader).Close();
    }

    public void Dispose()
    {
      ((IDisposable)reader).Dispose();
    }

    public bool GetBoolean(int i)
    {
      return ((IDataRecord)reader).GetBoolean(i);
    }

    public byte GetByte(int i)
    {
      return ((IDataRecord)reader).GetByte(i);
    }

    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
    {
      return ((IDataRecord)reader).GetBytes(i, fieldOffset, buffer, bufferoffset, length);
    }

    public char GetChar(int i)
    {
      return ((IDataRecord)reader).GetChar(i);
    }

    public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
    {
      return ((IDataRecord)reader).GetChars(i, fieldoffset, buffer, bufferoffset, length);
    }

    public IDataReader GetData(int i)
    {
      return ((IDataRecord)reader).GetData(i);
    }

    public string GetDataTypeName(int i)
    {
      return ((IDataRecord)reader).GetDataTypeName(i);
    }

    public DateTime GetDateTime(int i)
    {
      return ((IDataRecord)reader).GetDateTime(i);
    }

    public decimal GetDecimal(int i)
    {
      return ((IDataRecord)reader).GetDecimal(i);
    }

    public double GetDouble(int i)
    {
      return ((IDataRecord)reader).GetDouble(i);
    }

    public Type GetFieldType(int i)
    {
      return ((IDataRecord)reader).GetFieldType(i);
    }

    public float GetFloat(int i)
    {
      return ((IDataRecord)reader).GetFloat(i);
    }

    public Guid GetGuid(int i)
    {
      return ((IDataRecord)reader).GetGuid(i);
    }

    public short GetInt16(int i)
    {
      return ((IDataRecord)reader).GetInt16(i);
    }

    public int GetInt32(int i)
    {
      return ((IDataRecord)reader).GetInt32(i);
    }

    public long GetInt64(int i)
    {
      return ((IDataRecord)reader).GetInt64(i);
    }

    public string GetName(int i)
    {
      return ((IDataRecord)reader).GetName(i);
    }

    public int GetOrdinal(string name)
    {
      return ((IDataRecord)reader).GetOrdinal(name);
    }

    public DataTable GetSchemaTable()
    {
      return ((IDataReader)reader).GetSchemaTable();
    }

    public string GetString(int i)
    {
      return ((IDataRecord)reader).GetString(i);
    }

    public object GetValue(int i)
    {
      return ((IDataRecord)reader).GetValue(i);
    }

    public int GetValues(object[] values)
    {
      return ((IDataRecord)reader).GetValues(values);
    }

    public bool IsDBNull(int i)
    {
      return ((IDataRecord)reader).IsDBNull(i);
    }

    public bool NextResult()
    {
      return ((IDataReader)reader).NextResult();
    }

    public bool Read()
    {
      return ((IDataReader)reader).Read();
    }

    #endregion

    public void Fill(IDataReader reader)
    {
      Table.Load(reader);
      reader = Table.CreateDataReader();
    }
    public int Update()
    {
      return _dataAdapter.Update(data);
    }
  }
}
