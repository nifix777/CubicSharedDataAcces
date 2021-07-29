using Cubic.Shared.Data.Core.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;
using System.Xml;

namespace Cubic.Shared.Data.Core
{
  public class DbTypeMapping
  {
    protected readonly bool unicode;

    public DbTypeMapping(bool unicode)
    {
      this.unicode = unicode;
    }

    public static DbTypeMapping Default = new DbTypeMapping(false);

    //private static Dictionary<Type, DbType> @default;

    //public static Dictionary<Type, DbType> Default
    //{
    //  get
    //  {
    //    if (@default == null)
    //    {
    //      @default = new Dictionary<Type, DbType>()
    //      {
    //        { typeof(string), DbType.AnsiString },
    //        { typeof(bool), DbType.Boolean },
    //        { typeof(bool?), DbType.Boolean },
    //        { typeof(byte), DbType.Byte },
    //        { typeof(byte?), DbType.Byte },
    //        { typeof(byte[]), DbType.Binary },
    //        { typeof(char), DbType.AnsiStringFixedLength },
    //        { typeof(DateTime), DbType.DateTime },
    //        { typeof(DateTime?), DbType.DateTime },
    //        { typeof(DateTimeOffset), DbType.DateTimeOffset },
    //        { typeof(DateTimeOffset?), DbType.DateTimeOffset },
    //        { typeof(decimal?), DbType.Decimal },
    //        { typeof(decimal), DbType.Decimal },
    //        { typeof(double), DbType.Double },
    //        { typeof(double?), DbType.Double },
    //        { typeof(float), DbType.Single },
    //        { typeof(float?), DbType.Single },
    //        { typeof(Guid), DbType.Guid },
    //        { typeof(Guid?), DbType.Guid },
    //        { typeof(short), DbType.Int16 },
    //        { typeof(short?), DbType.Int16 },
    //        { typeof(int), DbType.Int32 },
    //        { typeof(int?), DbType.Int32 },
    //        { typeof(long), DbType.Int64 },
    //        { typeof(long?), DbType.Int64 },
    //        { typeof(object), DbType.Object },
    //        { typeof(TimeSpan), DbType.Time },
    //        { typeof(TimeSpan?), DbType.Time },
    //        { typeof(ushort), DbType.UInt16 },
    //        { typeof(ushort?), DbType.UInt16 },
    //        { typeof(uint), DbType.UInt32 },
    //        { typeof(uint?), DbType.UInt32 },
    //        { typeof(ulong), DbType.UInt64 },
    //        { typeof(ulong?), DbType.UInt64 },
    //      };
    //    };
    //    return @default;
    //  }
    //}
    //public DbType GetDbType(Type type)
    //{
    //  return Default[type];
    //}

    public virtual DbType GetDbType(TypeCode typecode)
    {
      switch (typecode)
      {
        case TypeCode.Boolean:
          return DbType.Boolean;
        case TypeCode.Byte:
          return DbType.Byte;
        case TypeCode.Char:
          return unicode ? DbType.StringFixedLength : DbType.AnsiStringFixedLength;
        case TypeCode.String:
          return unicode ? DbType.String : DbType.AnsiString;
        case TypeCode.DateTime:
          return DbType.DateTime;
        case TypeCode.Decimal:
          return DbType.Decimal;
        case TypeCode.Double:
          return DbType.Double;
        case TypeCode.Int16:
          return DbType.Int16;
        case TypeCode.Int32:
          return DbType.Int32;
        case TypeCode.Int64:
          return DbType.Int64;
        case TypeCode.Object:
          return DbType.Object;
        case TypeCode.SByte:
          return DbType.SByte;
        case TypeCode.Single:
          return DbType.Single;
        case TypeCode.UInt16:
          return DbType.UInt16;
        case TypeCode.UInt32:
          return DbType.UInt32;
        case TypeCode.UInt64:
          return DbType.UInt64;
        case TypeCode.DBNull:
        case TypeCode.Empty:
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public virtual DbType GetDbType(Type type)
    {
      if (typeof(string) == type) return unicode ? DbType.String : DbType.AnsiString;
      if (typeof(char[]) == type) return unicode ? DbType.StringFixedLength : DbType.AnsiStringFixedLength;
      if (typeof(bool) == type | typeof(bool?) == type) return DbType.Boolean;
      if (typeof(DateTime) == type | typeof(DateTime?) == type) return DbType.DateTime;
      if (typeof(DateTimeOffset) == type | typeof(DateTimeOffset?) == type) return DbType.DateTimeOffset;
      if (typeof(TimeSpan) == type | typeof(TimeSpan?) == type) return DbType.Time;
      if (typeof(Guid) == type | typeof(Guid?) == type) return DbType.Guid;
      if (typeof(BigInteger) == type | typeof(BigInteger?) == type) return DbType.VarNumeric;
      if (typeof(byte[]) == type) return DbType.Binary;
      if (typeof(byte) == type | typeof(byte?) == type) return DbType.Byte;
      if (typeof(sbyte) == type | typeof(sbyte?) == type) return DbType.SByte;
      if (typeof(short) == type | typeof(short?) == type) return DbType.Int16;
      if (typeof(int) == type | typeof(int?) == type) return DbType.Int32;
      if (typeof(long) == type | typeof(long?) == type) return DbType.Int64;
      if (typeof(ushort) == type | typeof(ushort?) == type) return DbType.UInt16;
      if (typeof(uint) == type | typeof(uint?) == type) return DbType.UInt32;
      if (typeof(ulong) == type | typeof(ulong?) == type) return DbType.UInt64;
      if (typeof(decimal) == type | typeof(decimal?) == type) return DbType.Decimal;
      if (typeof(float) == type | typeof(float?) == type) return DbType.Single;
      if (typeof(double) == type | typeof(double?) == type) return DbType.Double;
      else return DbType.Object;
    }

    public virtual Type GetClrType(DbType dbtype)
    {
      switch (dbtype)
      {
        case DbType.String:
        case DbType.AnsiString:
          return typeof(string);
        case DbType.AnsiStringFixedLength:
        case DbType.StringFixedLength:
          return typeof(char[]);
        case DbType.Binary:
          return typeof(byte[]); ;
        case DbType.Boolean:
          return typeof(bool);
        case DbType.Byte:
          return typeof(byte);
        case DbType.Currency:
        case DbType.Decimal:
          return typeof(decimal);
        case DbType.Date:
        case DbType.DateTime:
        case DbType.DateTime2:
          return typeof(DateTime); ;
        case DbType.DateTimeOffset:
          return typeof(DateTimeOffset);
        case DbType.Double:
          return typeof(double);
        case DbType.Guid:
          return typeof(Guid);
        case DbType.Int16:
          return typeof(short);
        case DbType.Int32:
          return typeof(int);
        case DbType.Int64:
          return typeof(long);
        case DbType.Object:
          return typeof(object);
        case DbType.SByte:
          return typeof(sbyte);
        case DbType.Single:
          return typeof(float);
        case DbType.Time:
          return typeof(TimeSpan);
        case DbType.UInt16:
          return typeof(ushort);
        case DbType.UInt32:
          return typeof(uint);
        case DbType.UInt64:
          return typeof(ulong);
        case DbType.VarNumeric:
          return typeof(BigInteger);
        case DbType.Xml:
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

  }
}
