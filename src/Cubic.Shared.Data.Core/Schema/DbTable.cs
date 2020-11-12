using System;
using System.Collections.Generic;
using System.Linq;

namespace Cubic.Shared.Data.Core.Schema
{
  public class DbTable : IDbTable
  {
    private readonly List<IDbColumn> _columns;

    public DbTable(string name)
    {
      Name = name;
      _columns = new List<IDbColumn>();
    }

    public DbTable(string name, IEnumerable<IDbColumn> columns) : this(name)
    {    
      _columns.AddRange(columns);
    }

    public string Name { get; }

    public IEnumerable<IDbColumn> Columns => _columns;

    public IEnumerable<IDbColumn> KeyColumns => _columns.Where(c => c.IsKey.HasValue && c.IsKey.Value);

    public IDbColumn RowVersion => _columns.FirstOrDefault(c => c.IsRowVersion.HasValue && c.IsRowVersion.Value);

    public bool HasUpdateableColumns => _columns.Any(c => c.IsReadOnly.HasValue && !c.IsReadOnly.Value);

    public void AddColumn<TValue>(string name, int ordinalPosition, bool isReadOnly, int maxLength, bool isKey, bool isIdentity, bool isRowVersion, bool isLong)
    {
      this.AddColumn(name, ordinalPosition, isReadOnly, maxLength, Nullable.GetUnderlyingType(typeof(TValue)) != null, isKey, isRowVersion, isIdentity, isLong, typeof(TValue));
    }

    public void AddColumn(string name, int ordinalPosition, bool isReadOnly, int maxLength, bool allowDBNull, bool isKey, bool isIdentity, bool isRowVersion, bool isLong, Type dataType)
    {
      this.AddColumn(new DbColumn(name, ordinalPosition, isReadOnly, maxLength, allowDBNull, isKey, isRowVersion, isIdentity, isLong, dataType, string.Empty));
    }

    public void AddColumn(IDbColumn column)
    {
      if (column is null)
      {
        throw new ArgumentNullException(nameof(column));
      }

      //column.NotNull(nameof(column));

      //if (string.IsNullOrEmpty(column.BaseColumnName))
      //{
      //  column.BaseColumnName = column.ColumnName;
      //}
      //if (string.IsNullOrEmpty(column.BaseTableName))
      //{
      //  column.BaseTableName = this.Name;
      //}

      this._columns.Add(column);
    }
  }
}
