using System;
using System.Collections.Generic;
using System.Text;

namespace Cubic.Shared.Data.Core.Sql
{
  public partial class Query
  {
    public Query Select(string field)
    {
      var select = new SqlAliasable(field);
      return this.With(q => q.Add(select));
    }

    public Query Select(params string[] fields)
    {
      foreach (var field in fields)
      {
        Select(field);
      }

      return this;
    }

    public Query WithLimit(int value)
    {
      return this.With(q => q.Limit = value);
    }

    public Query Orderby(string field, bool descending = true)
    {
      var sort = new Sort() { Field = new SqlAliasable(field), Descending = descending };
      return this.With(q => q.Add(sort));
    }
  }
}
