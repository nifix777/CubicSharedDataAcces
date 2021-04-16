using Cubic.Core;
using Cubic.Core.Diagnostics;
using Cubic.Core.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace Cubic.Shared.Data.Core.Schema
{
  public class DbInformation
  {
    private readonly static IDictionary<Type, string> _parameterPrefixCache = new Dictionary<Type, string>();

    private readonly DataTable _dbInfos;

    private readonly DataRow Main;

    private readonly DataColumn StringLiteralPatternColumn;

    private readonly DataColumn StatementSeparatorPatternColumn;

    private readonly DataColumn ParameterMarkerFormatColumn;

    private readonly DataColumn ParameterMarkerPatternColumn;

    private readonly DataColumn ParameterNamePatternColumn;

    private readonly DataColumn ParameterNameMaxLengthColumn;

    private readonly DataColumn DataSourceProductNameColumn;

    private readonly DataColumn DataSourceProductVersionColumn;

    private readonly Type _dbConnectionType;

    public static DbInformation Create(DbConnection connection)
    {
      connection.NotNull(nameof(connection));

      return new DbInformation(connection.GetSchema(DbMetaDataCollectionNames.DataSourceInformation), connection.GetType());
    }

    public DbInformation(DataTable dataSourceInforamtion, Type dbConnectionType)
    {
      _dbInfos = dataSourceInforamtion ?? throw new ArgumentNullException(nameof(dataSourceInforamtion));
      Main = _dbInfos.Rows[0];
      ParameterMarkerFormatColumn = _dbInfos.Columns[DbMetaDataColumnNames.ParameterMarkerFormat];
      ParameterNamePatternColumn = _dbInfos.Columns[DbMetaDataColumnNames.ParameterNamePattern];
      StatementSeparatorPatternColumn = _dbInfos.Columns[DbMetaDataColumnNames.StatementSeparatorPattern];
      StringLiteralPatternColumn = _dbInfos.Columns[DbMetaDataColumnNames.StringLiteralPattern];
      ParameterNameMaxLengthColumn = _dbInfos.Columns[DbMetaDataColumnNames.ParameterNameMaxLength];
      ParameterMarkerPatternColumn = _dbInfos.Columns[DbMetaDataColumnNames.ParameterMarkerPattern];
      DataSourceProductNameColumn = _dbInfos.Columns[DbMetaDataColumnNames.DataSourceProductName];
      DataSourceProductVersionColumn = _dbInfos.Columns[DbMetaDataColumnNames.DataSourceProductVersion];


      _dbConnectionType = dbConnectionType;
    }

    public string ParameterMarkerFormat => Main[ParameterMarkerFormatColumn].ToString();

    public string ParameterMarkerPattern => Main[ParameterMarkerPatternColumn].ToString();

    public string StatementSeparatorPattern => Main[StatementSeparatorPatternColumn].ToString();

    public string StringLiteralPattern => Main[StringLiteralPatternColumn].ToString();

    public string ParameterNamePattern => Main[ParameterNamePatternColumn].ToString();

    public int ParameterNameMaxLength => Main[ParameterNameMaxLengthColumn].ToInt32();

    public string ProductName => Main[DataSourceProductNameColumn].ToString();

    public string ProductVersionColumn => Main[DataSourceProductVersionColumn].ToString();

    public string GetParameterExpression(string parameterName, bool validate = false)
    {
      string format = _parameterPrefixCache.TryGetValue(_dbConnectionType, out format) ? format : ParameterMarkerFormat;
      var fullName = string.Format(format, parameterName.Trim());

      if (validate)
      {
        if (fullName.Length > ParameterNameMaxLength) throw new Exception($"Parametername with lenght '{fullName.Length}' is longer than allowed");

        var pattern = new Regex(ParameterMarkerPattern);

        if (!pattern.IsMatch(fullName)) throw new Exception($"Parametername does not match the provided Pattern");
      }

      return fullName;

    }

    public static void AddParamterPrefix(Type connectionType, string parameterPrefix)
    {
      _parameterPrefixCache[connectionType] = parameterPrefix;
    }


    public static void ClearCache()
    {
      _parameterPrefixCache.Clear();
    }


  }
}
