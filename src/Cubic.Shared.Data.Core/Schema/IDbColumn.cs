using System;
using System.Collections.Generic;
using System.Text;

namespace Cubic.Shared.Data.Core.Schema
{
  public interface IDbColumn
  {
    object this[string property] { get; }

    bool? AllowDBNull { get; }
    string BaseCatalogName { get;  }
    string BaseColumnName { get;  }
    string BaseSchemaName { get;  }
    string BaseServerName { get;  }
    string BaseTableName { get;  }
    string ColumnName { get;  }
    int? ColumnOrdinal { get;  }
    int? ColumnSize { get;  }
    Type DataType { get;  }

    object DefaultValue { get; }
    string DbTypeName { get; }
    bool? IsAliased { get;  }
    bool? IsAutoIncrement { get; }
    bool? IsExpression { get; }
    bool? IsHidden { get;  }
    //bool? IsIdentity { get; set; }
    bool? IsKey { get;  }
    bool? IsLong { get;  }
    bool? IsReadOnly { get;  }
    bool? IsRowVersion { get;  }
    bool? IsUnique { get;  }
    int? NumericPrecision { get;  }
    int? NumericScale { get;  }
  }
}
