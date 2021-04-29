using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Cubic.Shared.Data.Core.Caching
{
  public class CachedCommand : DbCommand
  {
    private readonly DataTable _cachedData;

    public CachedCommand(object cachedData, string commandText, DbParameterCollection dbParameters) : this(commandText, dbParameters)
    {
      _cachedData = new DataTable();
      _cachedData.Columns.Add("Result", cachedData.GetType());
      _cachedData.Rows.Add(cachedData);
    }
    public CachedCommand(DataTable cachedData, string commandText, DbParameterCollection dbParameters) : this(commandText, dbParameters)
    {
      _cachedData = cachedData;
    }

    private CachedCommand(string commandText, DbParameterCollection dbParameters)
    {
      CommandText = commandText;
      DbParameterCollection = dbParameters;
    }

    public override string CommandText { get; set; }
    public override int CommandTimeout { get; set; }
    public override CommandType CommandType { get; set; }
    public override bool DesignTimeVisible { get; set; }
    public override UpdateRowSource UpdatedRowSource { get; set; }
    protected override DbConnection DbConnection { get; set; }

    protected override DbParameterCollection DbParameterCollection { get; }

    protected override DbTransaction DbTransaction { get; set; }

    public override void Cancel()
    {
      _cachedData.Clear();
    }

    public override int ExecuteNonQuery()
    {
      throw new NotImplementedException();
    }

    public override object ExecuteScalar()
    {
      return _cachedData.Rows[0][0];
    }

    public override void Prepare()
    {
      
    }

    protected override void Dispose(bool disposing)
    {

      if(disposing)
      {
        _cachedData.Clear();
      }
      base.Dispose(disposing);
    }

    protected override DbParameter CreateDbParameter()
    {
      return Connection?.CreateCommand().CreateParameter();
    }

    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
      return new DataTableReader(_cachedData);
    }

    public static CachedCommand CacheReader(DbCommand command )
    {
      using(var reader = command.ExecuteReader())
      {
        var resultSet = new DataTable();
        resultSet.Load(reader);
        ////caching of Parametercollection could lead to issues if Parameter get removed or dot get released
        return new CachedCommand(resultSet, command.CommandText, new CachedParameterCollection());
      }
    }

    public static CachedCommand CacheScalar(DbCommand command)
    {
      var result = command.ExecuteScalar();
      ////caching of Parametercollection could lead to issues if Parameter get removed or dot get released
      return new CachedCommand(result, command.CommandText, new CachedParameterCollection());
    }

    internal class CachedParameterCollection : DbParameterCollection
    {
      private List<DbParameter> _items;



      public CachedParameterCollection(DbParameterCollection collection)
      {
        if(collection?.Count > 0)
        {
          foreach (var para in collection)
          {
            this.Add(para);
          }
        }
      }
      public CachedParameterCollection()
      {

      }

      public override int Count
      {
        get
        {
          return ((null != _items) ? _items.Count : 0);
        }
      }

      private List<DbParameter> InnerList
      {
        get
        {
          List<DbParameter> items = _items;

          if (null == items)
          {
            items = new List<DbParameter>();
            _items = items;
          }
          return items;
        }
      }

      public override bool IsFixedSize
      {
        get
        {
          return ((System.Collections.IList)InnerList).IsFixedSize;
        }
      }

      public override bool IsReadOnly
      {
        get
        {
          return ((System.Collections.IList)InnerList).IsReadOnly;
        }
      }

      public override bool IsSynchronized
      {
        get
        {
          return ((System.Collections.ICollection)InnerList).IsSynchronized;
        }
      }

      public override object SyncRoot
      {
        get
        {
          return ((System.Collections.ICollection)InnerList).SyncRoot;
        }
      }

      public override int Add(object value)
      {
        ValidateType(value);
        Validate(-1, value);
        InnerList.Add((DbParameter)value);
        return Count - 1;
      }

      public override void AddRange(System.Array values)
      {
        if (null == values)
        {
          throw new ArgumentNullException(nameof(values));
        }
        foreach (object value in values)
        {
          ValidateType(value);
        }
        foreach (DbParameter value in values)
        {
          Validate(-1, value);
          InnerList.Add((DbParameter)value);
        }
      }

      private int CheckName(string parameterName)
      {
        int index = IndexOf(parameterName);
        if (index < 0)
        {
          throw new IndexOutOfRangeException();
        }
        return index;
      }

      public override void Clear()
      {
        List<DbParameter> items = InnerList;

        if (null != items)
        {
          items.Clear();
        }
      }

      public override bool Contains(object value)
      {
        return (-1 != IndexOf(value));
      }

      public override bool Contains(string value)
      {
        return (-1 != IndexOf(value));
      }

      public override void CopyTo(Array array, int index)
      {
        ((System.Collections.ICollection)InnerList).CopyTo(array, index);
      }

      public override System.Collections.IEnumerator GetEnumerator()
      {
        return ((System.Collections.ICollection)InnerList).GetEnumerator();
      }

      protected override DbParameter GetParameter(int index)
      {
        RangeCheck(index);
        return InnerList[index];
      }

      protected override DbParameter GetParameter(string parameterName)
      {
        int index = IndexOf(parameterName);
        if (index < 0)
        {
          throw new IndexOutOfRangeException();
        }
        return InnerList[index];
      }

      private static int IndexOf(System.Collections.IEnumerable items, string parameterName)
      {
        if (null != items)
        {
          int i = 0;

          foreach (DbParameter parameter in items)
          {
            if (parameterName == parameter.ParameterName)
            {
              return i;
            }
            ++i;
          }
          i = 0;

          foreach (DbParameter parameter in items)
          {
            if (0 == string.CompareOrdinal(parameterName, parameter.ParameterName))
            {
              return i;
            }
            ++i;
          }
        }
        return -1;
      }

      public override int IndexOf(string parameterName)
      {
        return IndexOf(InnerList, parameterName);
      }

      public override int IndexOf(object value)
      {
        if (null != value)
        {
          ValidateType(value);

          List<DbParameter> items = InnerList;

          if (null != items)
          {
            int count = items.Count;

            for (int i = 0; i < count; i++)
            {
              if (value == items[i])
              {
                return i;
              }
            }
          }
        }
        return -1;
      }

      public override void Insert(int index, object value)
      {
        ValidateType(value);
        Validate(-1, (DbParameter)value);
        InnerList.Insert(index, (DbParameter)value);
      }

      private void RangeCheck(int index)
      {
        if ((index < 0) || (Count <= index))
        {
          throw new IndexOutOfRangeException();
        }
      }

      public override void Remove(object value)
      {

        ValidateType(value);
        int index = IndexOf(value);
        if (-1 != index)
        {
          RemoveIndex(index);
        }
      }

      public override void RemoveAt(int index)
      {
        RangeCheck(index);
        RemoveIndex(index);
      }

      public override void RemoveAt(string parameterName)
      {
        int index = CheckName(parameterName);
        RemoveIndex(index);
      }

      private void RemoveIndex(int index)
      {
        List<DbParameter> items = InnerList;
        Debug.Assert((null != items) && (0 <= index) && (index < Count), "RemoveIndex, invalid");
        DbParameter item = items[index];
        items.RemoveAt(index);
      }

      private void Replace(int index, object newValue)
      {
        List<DbParameter> items = InnerList;
        Debug.Assert((null != items) && (0 <= index) && (index < Count), "Replace Index invalid");
        ValidateType(newValue);
        Validate(index, newValue);
        DbParameter item = items[index];
        items[index] = (DbParameter)newValue;
      }

      protected override void SetParameter(int index, DbParameter value)
      {
        RangeCheck(index);
        Replace(index, value);
      }

      protected override void SetParameter(string parameterName, DbParameter value)
      {
        int index = IndexOf(parameterName);
        if (index < 0)
        {
          throw new IndexOutOfRangeException();
        }
        Replace(index, value);
      }

      private void Validate(int index, object value)
      {
        if (null == value)
        {
          throw new ArgumentNullException(nameof(value));
        }

        string name = ((DbParameter)value).ParameterName;
        if (0 == name.Length)
        {
          index = 1;
          do
          {
            name = name + index.ToString(CultureInfo.CurrentCulture);
            index++;
          } while (-1 != IndexOf(name));
          ((DbParameter)value).ParameterName = name;
        }
      }

      private void ValidateType(object value)
      {
        if (null == value)
        {
          throw new ArgumentNullException(nameof(value));
        }
        else if (!typeof(DbParameter).IsInstanceOfType(value))
        {
          throw new InvalidOperationException();
        }
      }
    }
  }
}
