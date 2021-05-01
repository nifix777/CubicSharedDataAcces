using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

    public static Task<IReadOnlyCollection<DataSourceInstance>> GetDataSourcesAsync(DbProviderFactory providerFactory, CancellationToken cancellation)
    {
      var tcs = new TaskCompletionSource<IReadOnlyCollection<DataSourceInstance>>();

      if (!providerFactory.CanCreateDataSourceEnumerator)
      {
        tcs.SetResult(new List<DataSourceInstance>());
      }
      else
      {
        Task.Run(() =>
        {
          var datasources = providerFactory.CreateDataSourceEnumerator()?.GetDataSources();

          tcs.SetResult(GetFromDataTable(datasources));
        }, cancellation).ConfigureAwait(false);
      }

      return tcs.Task;
    }

    public static IReadOnlyCollection<DataSourceInstance> GetFromDataTable(DataTable datasources)
    {
      var list = new List<DataSourceInstance>(datasources?.Rows.Count ?? 5);

      foreach (DataRow row in datasources.Rows)
      {
        var instancename = row.IsNull(1) ? string.Empty : row[1].ToString();
        var isclusterd = row.IsNull(2) ? (bool?)null : Convert.ToBoolean(row[2]);

        list.Add(new DataSourceInstance(row[0].ToString(), instancename, isclusterd, row[3].ToString(), row[4].ToString()));
      }

      return list.AsReadOnly();
    }

    public static DataTable GetSchemaTableFromDataTable(DataTable table)
    {
      if (table == null)
      {
        throw new ArgumentNullException("DataTable");
      }

      DataTable tempSchemaTable = new DataTable("SchemaTable");
      tempSchemaTable.Locale = System.Globalization.CultureInfo.InvariantCulture;

      DataColumn ColumnName = new DataColumn(SchemaTableColumn.ColumnName, typeof(System.String));
      DataColumn ColumnOrdinal = new DataColumn(SchemaTableColumn.ColumnOrdinal, typeof(System.Int32));
      DataColumn ColumnSize = new DataColumn(SchemaTableColumn.ColumnSize, typeof(System.Int32));
      DataColumn NumericPrecision = new DataColumn(SchemaTableColumn.NumericPrecision, typeof(System.Int16));
      DataColumn NumericScale = new DataColumn(SchemaTableColumn.NumericScale, typeof(System.Int16));
      DataColumn DataType = new DataColumn(SchemaTableColumn.DataType, typeof(System.Type));
      DataColumn ProviderType = new DataColumn(SchemaTableColumn.ProviderType, typeof(System.Int32));
      DataColumn IsLong = new DataColumn(SchemaTableColumn.IsLong, typeof(System.Boolean));
      DataColumn AllowDBNull = new DataColumn(SchemaTableColumn.AllowDBNull, typeof(System.Boolean));
      DataColumn IsReadOnly = new DataColumn(SchemaTableOptionalColumn.IsReadOnly, typeof(System.Boolean));
      DataColumn IsRowVersion = new DataColumn(SchemaTableOptionalColumn.IsRowVersion, typeof(System.Boolean));
      DataColumn IsUnique = new DataColumn(SchemaTableColumn.IsUnique, typeof(System.Boolean));
      DataColumn IsKeyColumn = new DataColumn(SchemaTableColumn.IsKey, typeof(System.Boolean));
      DataColumn IsAutoIncrement = new DataColumn(SchemaTableOptionalColumn.IsAutoIncrement, typeof(System.Boolean));
      DataColumn BaseSchemaName = new DataColumn(SchemaTableColumn.BaseSchemaName, typeof(System.String));
      DataColumn BaseCatalogName = new DataColumn(SchemaTableOptionalColumn.BaseCatalogName, typeof(System.String));
      DataColumn BaseTableName = new DataColumn(SchemaTableColumn.BaseTableName, typeof(System.String));
      DataColumn BaseColumnName = new DataColumn(SchemaTableColumn.BaseColumnName, typeof(System.String));
      DataColumn AutoIncrementSeed = new DataColumn(SchemaTableOptionalColumn.AutoIncrementSeed, typeof(System.Int64));
      DataColumn AutoIncrementStep = new DataColumn(SchemaTableOptionalColumn.AutoIncrementStep, typeof(System.Int64));
      DataColumn DefaultValue = new DataColumn(SchemaTableOptionalColumn.DefaultValue, typeof(System.Object));
      DataColumn Expression = new DataColumn(SchemaTableOptionalColumn.Expression, typeof(System.String));
      DataColumn ColumnMapping = new DataColumn(SchemaTableOptionalColumn.ColumnMapping, typeof(System.Data.MappingType));
      DataColumn BaseTableNamespace = new DataColumn(SchemaTableOptionalColumn.BaseTableNamespace, typeof(System.String));
      DataColumn BaseColumnNamespace = new DataColumn(SchemaTableOptionalColumn.BaseColumnNamespace, typeof(System.String));

      ColumnSize.DefaultValue = -1;

      if (table.DataSet != null)
        BaseCatalogName.DefaultValue = table.DataSet.DataSetName;

      BaseTableName.DefaultValue = table.TableName;
      BaseTableNamespace.DefaultValue = table.Namespace;
      IsRowVersion.DefaultValue = false;
      IsLong.DefaultValue = false;
      IsReadOnly.DefaultValue = false;
      IsKeyColumn.DefaultValue = false;
      IsAutoIncrement.DefaultValue = false;
      AutoIncrementSeed.DefaultValue = 0;
      AutoIncrementStep.DefaultValue = 1;


      tempSchemaTable.Columns.Add(ColumnName);
      tempSchemaTable.Columns.Add(ColumnOrdinal);
      tempSchemaTable.Columns.Add(ColumnSize);
      tempSchemaTable.Columns.Add(NumericPrecision);
      tempSchemaTable.Columns.Add(NumericScale);
      tempSchemaTable.Columns.Add(DataType);
      tempSchemaTable.Columns.Add(ProviderType);
      tempSchemaTable.Columns.Add(IsLong);
      tempSchemaTable.Columns.Add(AllowDBNull);
      tempSchemaTable.Columns.Add(IsReadOnly);
      tempSchemaTable.Columns.Add(IsRowVersion);
      tempSchemaTable.Columns.Add(IsUnique);
      tempSchemaTable.Columns.Add(IsKeyColumn);
      tempSchemaTable.Columns.Add(IsAutoIncrement);
      tempSchemaTable.Columns.Add(BaseCatalogName);
      tempSchemaTable.Columns.Add(BaseSchemaName);
      // specific to datatablereader
      tempSchemaTable.Columns.Add(BaseTableName);
      tempSchemaTable.Columns.Add(BaseColumnName);
      tempSchemaTable.Columns.Add(AutoIncrementSeed);
      tempSchemaTable.Columns.Add(AutoIncrementStep);
      tempSchemaTable.Columns.Add(DefaultValue);
      tempSchemaTable.Columns.Add(Expression);
      tempSchemaTable.Columns.Add(ColumnMapping);
      tempSchemaTable.Columns.Add(BaseTableNamespace);
      tempSchemaTable.Columns.Add(BaseColumnNamespace);

      foreach (DataColumn dc in table.Columns)
      {
        DataRow dr = tempSchemaTable.NewRow();

        dr[ColumnName] = dc.ColumnName;
        dr[ColumnOrdinal] = dc.Ordinal;
        dr[DataType] = dc.DataType;

        if (dc.DataType == typeof(string))
        {
          dr[ColumnSize] = dc.MaxLength;
        }

        dr[AllowDBNull] = dc.AllowDBNull;
        dr[IsReadOnly] = dc.ReadOnly;
        dr[IsUnique] = dc.Unique;

        if (dc.AutoIncrement)
        {
          dr[IsAutoIncrement] = true;
          dr[AutoIncrementSeed] = dc.AutoIncrementSeed;
          dr[AutoIncrementStep] = dc.AutoIncrementStep;
        }

        if (dc.DefaultValue != DBNull.Value)
          dr[DefaultValue] = dc.DefaultValue;

        dr[ColumnMapping] = dc.ColumnMapping;
        dr[BaseColumnName] = dc.ColumnName;
        dr[BaseColumnNamespace] = dc.Namespace;

        tempSchemaTable.Rows.Add(dr);
      }

      foreach (DataColumn key in table.PrimaryKey)
      {
        tempSchemaTable.Rows[key.Ordinal][IsKeyColumn] = true;
      }


      tempSchemaTable.AcceptChanges();

      return tempSchemaTable;
    }
  }
}
