using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cubic.Shared.Data.Core.Sql
{
  public partial class Query
  {
    public Query Where(Where where)
    {
      return this.With(q => q.Add(where));
    }

    public Query Or()
    {
      var lastCondition = this.Wheres.Last();
      lastCondition.IsAnd = false;
      return this;
    }

    public Query In(string field, params object[] values)
    {
      var where = new WhereIn() { Field = field, Values = new List<object>(values) };
      return this.Where(where);
    }

    public Query NotIn(string field, params object[] values)
    {
      var where = new WhereIn() { Field = field, Values = new List<object>(values), Invert = true };
      return this.Where(where);
    }

    public Query Value(string field, object value)
    {
      var where = new Where() { Field = field, Value = value };
      return this.Where(where);
    }

    public Query NotValue(string field, object value)
    {
      var where = new Where() { Field = field, Value = value, Invert = true };
      return this.Where(where);
    }

    public Query Same(string field, string otherField)
    {
      var where = new WhereField() { Left = field, Right = otherField };
      return this.Where(where);
    }

    public Query NotSame(string field, string otherField)
    {
      var where = new WhereField() { Left = field, Right = otherField, Invert = true };
      return this.Where(where);
    }
  }
}
