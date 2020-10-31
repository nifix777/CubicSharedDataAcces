using System;
using System.Collections.Generic;
using System.Text;

namespace Cubic.Shared.Data.Core.Schema
{
  public class DbColumn : IDbColumn
  {

    public DbColumn(string name, int? ordinalPosition, bool? isReadOnly, int? maxLength, bool? allowDBNull, bool? isKey, bool? isRowVersion, bool? isIdentity, bool? isLong, Type dataType)
    {
      this.ColumnName = name;
      this.ColumnOrdinal = ordinalPosition;
      IsReadOnly = isReadOnly;
      ColumnSize = maxLength;
      AllowDBNull = allowDBNull;
      IsKey = isKey;
      IsRowVersion = isRowVersion;
      IsIdentity = isIdentity;
      IsLong = isLong;
      DataType = dataType;
    }

    public bool? AllowDBNull { get; set; }
    public string BaseCatalogName { get; set; }
    public string BaseColumnName { get; set; }
    public string BaseSchemaName { get; set; }
    public string BaseServerName { get; set; }
    public string BaseTableName { get; set; }
    public string ColumnName { get; set; }
    public int? ColumnOrdinal { get; set; }
    public int? ColumnSize { get; set; }
    public bool? IsAliased { get; set; }
    public bool? IsAutoIncrement { get; set; }
    public bool? IsExpression { get; set; }
    public bool? IsHidden { get; set; }
    public bool? IsIdentity { get; set; }
    public bool? IsKey { get; set; }
    public bool? IsLong { get; set; }
    public bool? IsReadOnly { get; set; }
    public bool? IsUnique { get; set; }
    public bool? IsRowVersion { get; set; }
    public int? NumericPrecision { get; set; }
    public int? NumericScale { get; set; }
    //public string UdtAssemblyQualifiedName { get; set; }
    public Type DataType { get; set; }
    //public string DataTypeName { get; set; }
    public virtual object this[string property]
    {
      get
      {
        switch (property)
        {
          case nameof(AllowDBNull):
            return AllowDBNull;
          case nameof(BaseCatalogName):
            return BaseCatalogName;
          case nameof(BaseColumnName):
            return BaseColumnName;
          case nameof(BaseSchemaName):
            return BaseSchemaName;
          case nameof(BaseServerName):
            return BaseServerName;
          case nameof(BaseTableName):
            return BaseTableName;
          case nameof(ColumnName):
            return ColumnName;
          case nameof(ColumnOrdinal):
            return ColumnOrdinal;
          case nameof(ColumnSize):
            return ColumnSize;
          case nameof(IsAliased):
            return IsAliased;
          case nameof(IsAutoIncrement):
            return IsAutoIncrement;
          case nameof(IsExpression):
            return IsExpression;
          case nameof(IsHidden):
            return IsHidden;
          case nameof(IsIdentity):
            return IsIdentity;
          case nameof(IsKey):
            return IsKey;
          case nameof(IsLong):
            return IsLong;
          case nameof(IsReadOnly):
            return IsReadOnly;
          case nameof(IsUnique):
            return IsUnique;
          case nameof(NumericPrecision):
            return NumericPrecision;
          case nameof(NumericScale):
            return NumericScale;
          //case nameof(UdtAssemblyQualifiedName):
          //  return UdtAssemblyQualifiedName;
          case nameof(DataType):
            return DataType;
          //case nameof(DataTypeName):
          //  return DataTypeName;
          default:
            throw new ArgumentOutOfRangeException(nameof(property));
        }
      }
    }
  }
}
