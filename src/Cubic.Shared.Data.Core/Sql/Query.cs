using System;
using System.Collections.Generic;
using System.Text;

namespace Cubic.Shared.Data.Core.Sql
{

  public partial class Query
  {

    private readonly HashSet<SqlAliasable> _select;

    private readonly HashSet<Where> _where;

    private readonly HashSet<Sort> _sort;

    public Query(string name, int limit = 0) : this(limit, new From() { Value = new SqlAliasable(name) })
    {

    }

    public Query(int limit, From from)
    {
      Limit = limit;
      From = from ?? throw new ArgumentNullException(nameof(from));

      _select = new HashSet<SqlAliasable>();
      _where = new HashSet<Where>();
      _sort = new HashSet<Sort>();
    }

    public int Limit { get; set; }

    public From From { get; set; }

    public IEnumerable<SqlAliasable> Selects => _select;

    public void Add(SqlAliasable select) => _select.Add(select);

    public IEnumerable<Where> Wheres => _where;

    public void Add(Where where) => _where.Add(where);

    public IEnumerable<Sort> Sort => _sort;

    public void Add(Sort sort) => _sort.Add(sort);

    public Query With(Action<Query> callback)
    {
      callback(this);
      return this;
    }

  }
}
