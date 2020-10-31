using System.Collections.Generic;

namespace Cubic.Shared.Data.Core.Schema
{
  public interface IDbTable
  {
    string Name { get; }

    IEnumerable<IDbColumn> Columns { get; }

    IEnumerable<IDbColumn> KeyColumns { get; }

    IDbColumn RowVersion { get; }

    bool HasUpdateableColumns { get; }
  }
}
