using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public class CachingDbLookup : DbLookUp, IDbLookUpCache
  {
    private readonly IDictionary<int, object> _cache;

    public CachingDbLookup(DbConnection dbConnection, IDictionary<int, object> cache) : base(dbConnection)
    {
      _cache = cache ?? new Dictionary<int, object>();
    }

    public void ClearCache()
    {
      _cache.Clear();
    }

    protected override bool TryGetResult(DbCommand dbCommand, out object result)
    {
      result = null;
      var hash = DbCommandExtensions.GetHashCode(dbCommand, true);

      if(_cache.TryGetValue(hash, out result))
      {
        return true;
      }

      return false;
    }
  }
}
