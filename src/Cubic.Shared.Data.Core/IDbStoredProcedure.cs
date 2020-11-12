using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cubic.Shared.Data.Core
{
  public interface IDbStoredProcedure
  {
    string Name { get; }

    IList<IDbDataParameter> Parameters { get; }
    object InvokeScalar();
    Task<object> InvokeScalarAsync(CancellationToken cancellationToken = default);

    DbDataReader Invoke();
    Task<DbDataReader> InvokeAsync(CancellationToken cancellationToken = default);
  }
}
