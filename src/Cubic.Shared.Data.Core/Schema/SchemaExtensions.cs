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
  }
}
