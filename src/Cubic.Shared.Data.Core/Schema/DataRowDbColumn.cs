using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core.Schema
{
  internal class DataRowDbColumn : DbColumn
  {
    private DataColumnCollection _schemaColumns;
    private DataRow _schemaRow;

    public DataRowDbColumn(DataRow readerSchemaRow, DataColumnCollection readerSchemaColumns) : base(string.Empty, null, null, null, null, null, null, null, null, null)
    {
      _schemaRow = readerSchemaRow;
      _schemaColumns = readerSchemaColumns;
      populateFields();
    }

    private void populateFields()
    {
      AllowDBNull = GetIDbColumnValue<bool?>(SchemaTableColumn.AllowDBNull);
      BaseCatalogName = GetIDbColumnValue<string>(SchemaTableOptionalColumn.BaseCatalogName);
      BaseColumnName = GetIDbColumnValue<string>(SchemaTableColumn.BaseColumnName);
      BaseSchemaName = GetIDbColumnValue<string>(SchemaTableColumn.BaseSchemaName);
      BaseServerName = GetIDbColumnValue<string>(SchemaTableOptionalColumn.BaseServerName);
      BaseTableName = GetIDbColumnValue<string>(SchemaTableColumn.BaseTableName);
      ColumnName = GetIDbColumnValue<string>(SchemaTableColumn.ColumnName);
      ColumnOrdinal = GetIDbColumnValue<int?>(SchemaTableColumn.ColumnOrdinal);
      ColumnSize = GetIDbColumnValue<int?>(SchemaTableColumn.ColumnSize);
      IsAliased = GetIDbColumnValue<bool?>(SchemaTableColumn.IsAliased);
      IsAutoIncrement = GetIDbColumnValue<bool?>(SchemaTableOptionalColumn.IsAutoIncrement);
      IsExpression = GetIDbColumnValue<bool>(SchemaTableColumn.IsExpression);
      IsHidden = GetIDbColumnValue<bool?>(SchemaTableOptionalColumn.IsHidden);
      //IsIdentity = GetIDbColumnValue<bool?>("IsIdentity");
      IsKey = GetIDbColumnValue<bool?>(SchemaTableColumn.IsKey);
      IsLong = GetIDbColumnValue<bool?>(SchemaTableColumn.IsLong);
      IsReadOnly = GetIDbColumnValue<bool?>(SchemaTableOptionalColumn.IsReadOnly);
      IsUnique = GetIDbColumnValue<bool?>(SchemaTableColumn.IsUnique);
      IsRowVersion = GetIDbColumnValue<bool?>(SchemaTableOptionalColumn.IsRowVersion);
      NumericPrecision = GetIDbColumnValue<int?>(SchemaTableColumn.NumericPrecision);
      NumericScale = GetIDbColumnValue<int?>(SchemaTableColumn.NumericScale);
      //UdtAssemblyQualifiedName = GetIDbColumnValue<string>("UdtAssemblyQualifiedName");
      DataType = GetIDbColumnValue<Type>(SchemaTableColumn.DataType);
      DbTypeName = GetIDbColumnValue<string>(SchemaTableColumn.DataType);
      DefaultValue = GetIDbColumnValue<object>(SchemaTableOptionalColumn.DefaultValue);
    }

    private T GetIDbColumnValue<T>(string columnName)
    {
      if (!_schemaColumns.Contains(columnName))
      {
        return default(T);
      }

      object schemaObject = _schemaRow[columnName];
      if (schemaObject is T)
      {
        return (T)schemaObject;
      }
      return default(T);
    }
  }
}
