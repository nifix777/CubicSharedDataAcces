using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public class DbRecordset : DbUpdateableDataRecordSet
  {
    private readonly DataTable table;

    public DbRecordset(DataTable table)
    {
      this.table = table;
      this.table.RowChanged += DataChanged;
    }

    private int rowCounter = -1;
    private bool isOpen = true;
    private bool reachEORows = false;
    private DataRow currentDataRow = null;
    private bool currentRowRemoved;
    private bool ReaderIsInvalid;
    private bool IsSchemaChanged;
    private DataTable schemaTable;

    public override object this[int i]
    {
      get
      {
        ValidateState();
        return currentDataRow[i];
      }
    }

    public override object this[string name]
    {
      get
      {
        ValidateState();
        return currentDataRow[name];
      }
    }

    public override int FieldCount
    {
      get
      {
        ValidateState();
        return table.Columns.Count;
      }
    }

    public override int Depth => 0;

    public override bool HasRows
    {
      get
      {
        ValidateOpen();
        ValidateReader();
        return table.Rows.Count > 0;
      }
    }

    public override bool IsClosed => !isOpen;

    public override int RecordsAffected => 0;

    public override void Close()
    {
      if (!isOpen)
        return;

      schemaTable = null;

      isOpen = false;
    }

    public override bool GetBoolean(int i)
    {
      return (bool)this[i];
    }

    public override byte GetByte(int i)
    {
      return (byte)this[i];
    }

    public override long GetBytes(int i, long dataIndex, byte[] buffer, int bufferIndex, int length)
    {
      var tempBuffer = (byte[])this[i];

      if (buffer == null)
      {
        return tempBuffer.Length;
      }
      int srcIndex = (int)dataIndex;
      int byteCount = Math.Min(tempBuffer.Length - srcIndex, length);
      if (srcIndex < 0)
      {
        throw new ArgumentOutOfRangeException("dataIndex");
      }
      else if ((bufferIndex < 0) || (bufferIndex > 0 && bufferIndex >= buffer.Length))
      {
        throw new ArgumentOutOfRangeException("bufferIndex");
      }

      if (0 < byteCount)
      {
        Array.Copy(tempBuffer, dataIndex, buffer, bufferIndex, byteCount);
      }
      else if (length < 0)
      {
        throw new IndexOutOfRangeException();
      }
      else
      {
        byteCount = 0;
      }
      return byteCount;
    }

    public override char GetChar(int i)
    {
      return (char)this[i];
    }

    public override long GetChars(int i, long dataIndex, char[] buffer, int bufferIndex, int length)
    {
      var tempBuffer = (char[])this[i];

      if (buffer == null)
      {
        return tempBuffer.Length;
      }
      int srcIndex = (int)dataIndex;
      int byteCount = Math.Min(tempBuffer.Length - srcIndex, length);
      if (srcIndex < 0)
      {
        throw new ArgumentOutOfRangeException("dataIndex");
      }
      else if ((bufferIndex < 0) || (bufferIndex > 0 && bufferIndex >= buffer.Length))
      {
        throw new ArgumentOutOfRangeException("bufferIndex");
      }

      if (0 < byteCount)
      {
        Array.Copy(tempBuffer, dataIndex, buffer, bufferIndex, byteCount);
      }
      else if (length < 0)
      {
        throw new IndexOutOfRangeException();
      }
      else
      {
        byteCount = 0;
      }
      return byteCount;
    }

    public override string GetDataTypeName(int i)
    {
      return GetFieldType(i).Name;
    }

    public override DateTime GetDateTime(int i)
    {
      return (DateTime)this[i];
    }

    public override decimal GetDecimal(int i)
    {
      return (decimal)this[i];
    }

    public override double GetDouble(int i)
    {
      return (double)this[i];
    }

    public override Type GetFieldType(int i)
    {
      ValidateOpen();
      ValidateReader();
      return (table.Columns[i].DataType);
    }

    public override float GetFloat(int i)
    {
      return (float)this[i];
    }

    public override Guid GetGuid(int i)
    {
      return (Guid)this[i];
    }

    public override short GetInt16(int i)
    {
      return (short)this[i];
    }

    public override int GetInt32(int i)
    {
      return (int)this[i];
    }

    public override long GetInt64(int i)
    {
      return (long)this[i];
    }

    public override string GetName(int i)
    {
      ValidateOpen();
      ValidateReader();
      return (table.Columns[i].ColumnName);
    }

    public override int GetOrdinal(string name)
    {
      ValidateOpen();
      ValidateReader();
      DataColumn dc = table.Columns[name];

      if (dc != null)
      {
        return dc.Ordinal;
      }
      else
      {
        throw new IndexOutOfRangeException();
      }
    }

    public override DataTable GetSchemaTable()
    {
      ValidateOpen();
      ValidateReader();

      // each time, we just get schema table of current table for once, no need to recreate each time, if schema is changed, reader is already
      // is invalid
      if (schemaTable == null)
        schemaTable = Schema.SchemaExtensions.GetSchemaTableFromDataTable(table);

      return schemaTable;
    }

    public override string GetString(int i)
    {
      return (string)this[i];
    }

    public override object GetValue(int i)
    {
      return this[i];
    }

    public override int GetValues(object[] values)
    {
      ValidateState();
      Array.Copy(currentDataRow.ItemArray, values, currentDataRow.ItemArray.Length > values.Length ? values.Length : currentDataRow.ItemArray.Length);
      return (currentDataRow.ItemArray.Length > values.Length ? values.Length : currentDataRow.ItemArray.Length);
    }

    public override bool IsDBNull(int i)
    {
      ValidateState();
      return currentDataRow.IsNull(i);
    }

    override public Type GetProviderSpecificFieldType(int ordinal)
    {
      ValidateOpen();
      ValidateReader();
      return GetFieldType(ordinal);
    }

    override public Object GetProviderSpecificValue(int ordinal)
    {
      ValidateOpen();
      ValidateReader();
      return GetValue(ordinal);
    }

    override public int GetProviderSpecificValues(object[] values)
    {
      ValidateOpen();
      ValidateReader();
      return GetValues(values);
    }

    private void ValidateState()
    {
      ValidateOpen();
      ValidateReader();
      ValidateRow(rowCounter);
    }

    private void ValidateOpen()
    {
      if (!isOpen)
        throw new InvalidOperationException("DataReader is closed");

    }

    private void ValidateReader()
    {
      if (ReaderIsInvalid)
        throw new InvalidOperationException();

      if (IsSchemaChanged)
      {
        throw new InvalidOperationException("Schema Changed"); // may be we can use better error message!
      }

    }

    internal void DataTableCleared()
    {
      if (!isOpen)
        return;

      rowCounter = -1;
      if (!reachEORows)
        currentRowRemoved = true;
    }
    private void ValidateRow(int rowPosition)
    {
      if (ReaderIsInvalid)
        throw new InvalidOperationException();

      if (0 > rowPosition || table.Rows.Count <= rowPosition)
      {
        ReaderIsInvalid = true;
        throw new IndexOutOfRangeException();
      }
    }

    internal void DataChanged(object sender, DataRowChangeEventArgs args)
    {
      if ((!isOpen) || (rowCounter == -1))
        return;
      /*           if (rowCounter == -1 && tableCleared && args.Action == DataRowAction.Add) {
                     tableCleared = false;
                     return;
                 }
      */
      switch (args.Action)
      {
        case DataRowAction.Add:
          ValidateRow(rowCounter + 1);
          /*                   if (tableCleared) {
                                 tableCleared = false;
                                 rowCounter++;
                                 currentDataRow = currentDataTable.Rows[rowCounter];
                                 currentRowRemoved = false;
                             }
                             else 
          */
          if (currentDataRow == table.Rows[rowCounter + 1])
          { // check if we moved one position up
            rowCounter++;  // if so, refresh the datarow and fix the counter
          }
          break;
        case DataRowAction.Delete: // delete
        case DataRowAction.Rollback:// rejectchanges
        case DataRowAction.Commit: // acceptchanges
          if (args.Row.RowState == DataRowState.Detached)
          {
            if (args.Row != currentDataRow)
            {
              if (rowCounter == 0) // if I am at first row and no previous row exist,NOOP
                break;
              ValidateRow(rowCounter - 1);
              if (currentDataRow == table.Rows[rowCounter - 1])
              { // one of previous rows is detached, collection size is changed!
                rowCounter--;
              }
            }
            else
            { // we are proccessing current datarow
              currentRowRemoved = true;
              if (rowCounter > 0)
              {  // go back one row, no matter what the state is
                rowCounter--;
                currentDataRow = table.Rows[rowCounter];
              }
              else
              {  // we are on 0th row, so reset data to initial state!
                rowCounter = -1;
                currentDataRow = null;
              }
            }
          }
          break;
        default:
          break;
      }
    }

    #region DbUpdateableDataRecordSet
    protected override void SetRecordValue(int ordinal, object value)
    {
      if (value is IDataRecord record)
      {
        throw new NotImplementedException();
      }
      else
      {
        currentDataRow[ordinal] = value;
      }
    }

    public override IEnumerator GetEnumerator()
    {
      ValidateOpen();
      return new DbEnumerator(this);
    }

    public override bool NextResult()
    {
      ValidateOpen();

      return true;
    }

    public override bool Read()
    {
      if (!isOpen)
      {
        isOpen = true;
      }

      ValidateOpen();

      ValidateReader();


      if (reachEORows)
      {
        return false;
      }

      if (rowCounter >= table.Rows.Count - 1)
      {
        reachEORows = true;
        return false;
      }

      rowCounter++;
      ValidateRow(rowCounter);
      currentDataRow = table.Rows[rowCounter];

      while (currentDataRow.RowState == DataRowState.Deleted)
      {
        rowCounter++;
        if (rowCounter == table.Rows.Count)
        {
          reachEORows = true;
          return false;
        }
        ValidateRow(rowCounter);
        currentDataRow = table.Rows[rowCounter];
      }
      if (currentRowRemoved)
        currentRowRemoved = false;

      return true;
    }
    #endregion


  }
}
