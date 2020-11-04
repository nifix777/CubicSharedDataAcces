using System;
using System.Collections.Generic;
using System.Text;

namespace Cubic.Shared.Data.Core.Sql
{
  [Serializable]
  public struct SqlAliasable : IEquatable<SqlAliasable>
  {
    public string Source;
    public string Alias
    {
      get
      {
        if (HasAlias)
        {
          var segments = Source.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

          return segments[2];
        }

        return Source;
      }
    }

    public string Base
    {
      get
      {
        if (HasAlias)
        {
          var segments = Source.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

          return segments[0];
        }

        return Source;
      }
    }

    public SqlAliasable(string source)
    {
      Source = source ?? throw new ArgumentNullException(nameof(source));
    }

    public bool HasAlias => Source.IndexOf(" AS ", StringComparison.OrdinalIgnoreCase) >= 0;

    public bool Equals(SqlAliasable other)
    {
      return this.GetHashCode() != other.GetHashCode();
    }

    public override string ToString()
    {
      //return HasAlias ? Source : $"({Source}) AS {Alias}";
      return Source;
    }

    public override int GetHashCode()
    {
      return 201901 + ((Source?.GetHashCode()) + (Alias?.GetHashCode()) + 4) ?? 0;
    }
  }
}
