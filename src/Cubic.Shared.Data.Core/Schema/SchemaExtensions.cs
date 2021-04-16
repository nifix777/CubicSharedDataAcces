using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core.Schema
{
  public static class SchemaExtensions
  {
    public static IEnumerable<IDbColumn> GetColumnSchema(this IDataReader reader)
    {
      DataTable schemaTable = reader.GetSchemaTable();
      DataColumnCollection schemaTableColumns = schemaTable.Columns;
      foreach (DataRow row in schemaTable.Rows)
      {
        yield return new DataRowDbColumn(row, schemaTableColumns);
      }
    }

    public static System.Collections.ObjectModel.ReadOnlyCollection<IDbColumn> GetTableSchema(this DbConnection connection, string tablename)
    {
      List<IDbColumn> columnSchema = new List<IDbColumn>();

      // Specify the restrictions.  
      string[] restrictions = new string[4];
      restrictions[1] = connection.Database;
      restrictions[2] = tablename;

      DataTable schemaTable = connection.GetSchema(DbMetaDataCollectionNames.MetaDataCollections, restrictions);
      DataColumnCollection schemaTableColumns = schemaTable.Columns;
      foreach (DataRow row in schemaTable.Rows)
      {
        DbColumn dbColumn = new DataRowDbColumn(row, schemaTableColumns);
        columnSchema.Add(dbColumn);
      }
      return new System.Collections.ObjectModel.ReadOnlyCollection<IDbColumn>(columnSchema);
    }

    public static IDbTable GetTableInfo(this DbConnection connection, string tablename)
    {
      return new DbTable(tablename, connection.GetTableSchema(tablename));
    }

    public static IReadOnlyList<IDbTable> GetTableInfos(this DbConnection connection)
    {
      List<IDbTable> tables = new List<IDbTable>();
      // Specify the restrictions.  
      string[] restrictions = new string[4];
      restrictions[1] = connection.Database;

      DataTable schemaTable = connection.GetSchema(DbMetaDataCollectionNames.MetaDataCollections, restrictions);
      foreach (DataRow row in schemaTable.Rows)
      {
        tables.Add(connection.GetTableInfo(row[DbMetaDataColumnNames.CollectionName].ToString()));
      }

      return tables.AsReadOnly();
      
    }

    public static bool CanGetColumnSchema(this IDataReader reader)
    {
      return reader is DbDataReader;
    }
  }
}
