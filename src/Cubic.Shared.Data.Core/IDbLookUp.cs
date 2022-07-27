using System.Collections.Generic;

namespace Cubic.Shared.Data.Core
{
  public interface IDbLookUp
  {
    bool DataExists(string column, string source, string clause, IEnumerable<KeyValuePair<string, object>> parameters = null);
    T DataExists<T>(string column, string source, string clause, IEnumerable<KeyValuePair<string, object>> parameters = null);
    IDictionary<string, object> GetData(IEnumerable<string> columns, string source, string clause, IEnumerable<KeyValuePair<string, object>> parameters = null);
    object GetValue(string column, string source, string clause, IEnumerable<KeyValuePair<string, object>> parameters = null);
    T GetValue<T>(string column, string source, string clause, IEnumerable<KeyValuePair<string, object>> parameters = null);
  }

  public interface IDbLookUpCache
  {
    void ClearCache();
  }
}