using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Cubic.Shared.Data.Core.Sql
{
  public static class SqlHelper
  {

    static public readonly string SqlValueError = "<Unable to convert as string>";

    static readonly Regex _goPattern = new Regex(@"^\s*GO(?:\s|$)+", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);


    static public TextWriter CommandAsText(TextWriter w, IDbCommand cmd)
    {
      if (cmd.CommandType == CommandType.StoredProcedure)
      {
        w.Write($"exec {cmd.CommandText} <= ");
        WriteCallParameters(w, cmd.Parameters);
      }
      else
      {
        WriteCallParameters(w, cmd.Parameters);
        w.Write(" => ");
        w.Write(cmd.CommandText);
      }
      return w;
    }


    static public string CommandAsText(DbCommand cmd)
    {
      StringWriter w = new StringWriter();
      CommandAsText(w, cmd);
      return w.GetStringBuilder().ToString();
    }


    static public TextWriter WriteCallParameters(TextWriter w, IDataParameterCollection c)
    {
      bool atLeastOne = false;
      foreach (IDataParameter p in c)
      {
        if (p.Direction != ParameterDirection.ReturnValue)
        {
          if (atLeastOne) w.Write(", ");
          atLeastOne = true;
          w.Write(p.ParameterName);
          w.Write('=');
          w.Write(SqlValue(p.Value, p.DbType));
          if (p.Direction != ParameterDirection.Input) w.Write(" output");
        }
      }
      return w;
    }
    static public string SqlValue(object v, DbType dbType, bool throwError = false)
    {
      if (v == null || v == DBNull.Value) return "null";
      try
      {
        switch (dbType)
        {
          case DbType.String: return string.Format("N'{0}'", SqlEncodeStringContent(Convert.ToString(v, CultureInfo.InvariantCulture)));
          case DbType.Int32: return Convert.ToString(v, CultureInfo.InvariantCulture);
          case DbType.Boolean: return Convert.ToBoolean(v) ? "1" : "0";
          case DbType.StringFixedLength: goto case SqlDbType.VarChar;
          case DbType.AnsiString: return string.Format("'{0}'", SqlEncodeStringContent(Convert.ToString(v, CultureInfo.InvariantCulture)));
          case DbType.AnsiStringFixedLength: goto case DbType.AnsiString;
          case DbType.DateTime: return string.Format("convert( DateTime, '{0:s}', 126 )", v);
          case DbType.DateTime2: return string.Format("'{0:O}'", v);
          case DbType.Time: return string.Format("'{0:c}'", v);
          case DbType.Byte: return Convert.ToString(v, CultureInfo.InvariantCulture);
          case DbType.Guid: return ((Guid)v).ToString("B");
          case DbType.Int16: return Convert.ToString(v, CultureInfo.InvariantCulture);
          //case DbType.SmallDateTime: return String.Format("convert( SmallDateTime, '{0:s}', 126 )", v);
          case DbType.Int64: return Convert.ToString(v, CultureInfo.InvariantCulture);
          //case DbType.NText: goto case SqlDbType.NVarChar;
          case DbType.Double: return Convert.ToString(v, CultureInfo.InvariantCulture);
          case DbType.Single: return Convert.ToString(v, CultureInfo.InvariantCulture);
          case DbType.Decimal: return Convert.ToString(v, CultureInfo.InvariantCulture);
          case DbType.Xml: return string.Format("cast( '{0}' as xml )", SqlEncodeStringContent(Convert.ToString(v, CultureInfo.InvariantCulture)));
          case DbType.Object: return Convert.ToString(v, CultureInfo.InvariantCulture);
          case DbType.Binary:
            {
              byte[] bytes = v as byte[];
              if (bytes == null)
              {
                if (throwError) throw new Exception($"Unable to convert '{v.GetType()}' to byte[] to compute sql string representation for {dbType}.");
                return SqlValueError;
              }
              StringBuilder b = new StringBuilder("0x");
              for (int i = 0; i < bytes.Length; i++)
              {
                const string c = "0123456789ABCDEF";
                b.Append(c[bytes[i] >> 4]);
                b.Append(c[bytes[i] & 0x0F]);
              }
              return b.ToString();
            }
          default:
            {
              if (throwError) throw new Exception($"No sql string representation for: {dbType}");
              return SqlValueError;
            }
        }
      }
      catch (Exception)
      {
        if (throwError) throw;
        return SqlValueError;
      }
    }


    static public string SqlEncodeStringContent(string s)
    {
      return s == null ? string.Empty : s.Replace("'", "''");
    }


    static public IEnumerable<string> SplitGoSeparator(string script)
    {
      if (!string.IsNullOrWhiteSpace(script))
      {
        int curBeg = 0;
        for (Match goDelim = _goPattern.Match(script); goDelim.Success; goDelim = goDelim.NextMatch())
        {
          int lenScript = goDelim.Index - curBeg;
          if (lenScript > 0)
          {
            yield return script.Substring(curBeg, lenScript);
          }
          curBeg = goDelim.Index + goDelim.Length;
        }
        if (script.Length > curBeg)
        {
          yield return script.Substring(curBeg).TrimEnd();
        }
      }
    }
  }
}
