using System;
using System.Collections.Generic;
using System.Text;

namespace Cubic.Shared.Data.Core.Sql
{
  public static class QueryExtensions
  {

    public static Query Select(this Query query, string field)
    {
      return query.With(q => q.Add(new SqlAliasable(field)));
    }

    //public static Query From(this Query query, string field)
    //{
    //  return query.With(q => q.From(new SqlAliasable(field)));
    //}

    public static Query Select(this Query query, IEnumerable<string> fields)
    {
      foreach (var field in fields)
      {
        query.Add(new SqlAliasable(field));
      }

      return query;
    }
  }
}
