using Cubic.Shared.Data.Core.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public class DbTypeMapping
  {
    private static Dictionary<Type, DbType> @default;

    public static Dictionary<Type, DbType> Default
    {
      get
      {
        if (@default == null)
        {
          @default = new Dictionary<Type, DbType>()
          {
            { typeof(string), DbType.String },
            { typeof(bool), DbType.Boolean },
            { typeof(bool?), DbType.Boolean },
            { typeof(byte), DbType.Byte },
            { typeof(byte?), DbType.Byte },
            { typeof(byte[]), DbType.Binary },
            //{ typeof(char), DbType.Boolean },
            { typeof(DateTime), DbType.DateTime },
            { typeof(DateTime?), DbType.DateTime },
            { typeof(DateTimeOffset), DbType.DateTimeOffset },
            { typeof(DateTimeOffset?), DbType.DateTimeOffset },
            { typeof(decimal?), DbType.Decimal },
            { typeof(decimal), DbType.Decimal },
            { typeof(double), DbType.Double },
            { typeof(double?), DbType.Double },
            { typeof(float), DbType.Single },
            { typeof(float?), DbType.Single },
            { typeof(Guid), DbType.Guid },
            { typeof(Guid?), DbType.Guid },
            { typeof(short), DbType.Int16 },
            { typeof(short?), DbType.Int16 },
            { typeof(int), DbType.Int32 },
            { typeof(int?), DbType.Int32 },
            { typeof(long), DbType.Int64 },
            { typeof(long?), DbType.Int64 },
            { typeof(object), DbType.Object },
            { typeof(TimeSpan), DbType.Time },
            { typeof(TimeSpan?), DbType.Time },
            { typeof(ushort), DbType.UInt16 },
            { typeof(ushort?), DbType.UInt16 },
            { typeof(uint), DbType.UInt32 },
            { typeof(uint?), DbType.UInt32 },
            { typeof(ulong), DbType.UInt64 },
            { typeof(ulong?), DbType.UInt64 },
          };
        };
        return @default;
      }
    }
    public static DbType GetDbType(Type type)
    {
      return Default[type];
    }


  }
}
