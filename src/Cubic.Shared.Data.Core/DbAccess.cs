using Cubic.Core;
using Cubic.Core.Collections;
using Cubic.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Text;

namespace Cubic.Shared.Data.Core
{
  public class DbAccess : DataAccess, IDbAccess
  {
    const string ConnectionStringKey = "ConnectionString";

    private void Initialize()
    {
      if (!Contains(nameof(DefaultCommandBehavior)))
      {
        DefaultCommandBehavior = CommandBehavior.Default;
      }

      if (!Contains(nameof(EnhancedTimeout)))
      {
        EnhancedTimeout = 60;
      }
    }

    public DbAccess()
    {
      ConnectionStringBuilder = new DbConnectionStringBuilder();
      Initialize();
    }
    public DbAccess(string dbstring, string accesstring) : base(accesstring)
    {
      ConnectionStringBuilder = new DbConnectionStringBuilder();
      ConnectionString = dbstring;

      Initialize();
    }

    protected DbAccess(string dbstring, string accesstring, DbConnectionStringBuilder builder) : base(accesstring)
    {
      ConnectionStringBuilder = new DbConnectionStringBuilder();
      ConnectionString = dbstring;

      Initialize();
    }

    public DbAccess(string name/*, DatabaseType dbType*/) : this(AccessorType.Database, name, /*dbType,*/ new DbConnectionStringBuilder())
    {

    }

    protected DbAccess(AccessorType accessorType, string name, /*DatabaseType dbType,*/ DbConnectionStringBuilder connectionStringDatabaseBuilder) : base(accessorType, name)
    {
      ConnectionStringBuilder = connectionStringDatabaseBuilder;
      //DatabaseType = dbType;

      Initialize();
    }

    public DbAccess(SerializationInfo info, StreamingContext context) : base()
    {
      ConnectionStringBuilder.ConnectionString = info.GetString(ConnectionStringKey);
    }

    protected DbConnectionStringBuilder ConnectionStringBuilder { get; set; }

    protected TBuilder Builder<TBuilder>() where TBuilder : DbConnectionStringBuilder
    {
      return (TBuilder)ConnectionStringBuilder;
    }

    protected DbAccess CreateInternal(DbConnectionStringBuilder builder) => new DbAccess(string.Empty, string.Empty, builder);

    public string ConnectionString
    {
      get => ConnectionStringBuilder.ConnectionString;

      set => ConnectionStringBuilder.ConnectionString = value;
    }

    //public DatabaseType DatabaseType
    //{
    //  //get => this[nameof(DatabaseType)].ToString().ToEnum<DatabaseType>();
    //  get => this.GetValueOrDefault(nameof(DatabaseType), DatabaseType.Individual).ToEnum<DatabaseType>();
    //  set => this[nameof(DatabaseType)] = value;
    //}

    public virtual int Timeout
    {
      get => ConnectionStringBuilder["Connection Timeout"].ToInt32();
      set => ConnectionStringBuilder["Connection Timeout"] = value;
    }

    public virtual string ApplicationName
    {
      get => ConnectionStringBuilder[nameof(ApplicationName)].ToString();
      set => ConnectionStringBuilder[nameof(ApplicationName)] = value;
    }

    public virtual string Client
    {
      get => ConnectionStringBuilder[nameof(Client)].ToString();
      set => ConnectionStringBuilder[nameof(Client)] = value;
    }

    public int EnhancedTimeout
    {
      //get => this[nameof(EnhancedTimeout)].ToInt32();
      get => this.GetValueOrDefault(nameof(EnhancedTimeout), 60).ToInt32();
      set => this[nameof(EnhancedTimeout)] = value;
    }

    public CommandBehavior DefaultCommandBehavior
    {
      //get => this[nameof(DefaultCommandBehavior)].ToString().ToEnum<CommandBehavior>();
      get => this.GetValueOrDefault(nameof(DefaultCommandBehavior), CommandBehavior.Default).ToEnum<CommandBehavior>();
      set => this[nameof(DefaultCommandBehavior)] = value;
    }

    //public virtual bool MultipleActiveResultSets
    //{
    //  get { throw new NotImplementedException(); }
    //  set { throw new NotImplementedException(); }
    //}

    public virtual string Server
    {
      get { return ConnectionStringBuilder[nameof(Server)].ToString(); }
      set { ConnectionStringBuilder[nameof(Server)] = value; }
    }

    public virtual string Database
    {
      get { return ConnectionStringBuilder[nameof(Database)].ToString(); }
      set { ConnectionStringBuilder[nameof(Database)] = value; }
    }

    public virtual string UserId
    {
      get { return ConnectionStringBuilder[nameof(UserId)].ToString(); }
      set { ConnectionStringBuilder[nameof(UserId)] = value; }
    }

    public virtual string Password
    {
      get { return ConnectionStringBuilder[nameof(Password)].ToString(); }
      set { ConnectionStringBuilder[nameof(Password)] = value; }
    }

    public virtual bool IntegratedSecurity
    {
      get { return ConnectionStringBuilder.GetValueOrDefault(nameof(IntegratedSecurity), false).ToBool(); }
      set { ConnectionStringBuilder[nameof(IntegratedSecurity)] = value; }
    }

    public virtual int Port
    {
      get { return ConnectionStringBuilder.GetValueOrDefault(nameof(Port), 0).ToInt32(); }
      set { ConnectionStringBuilder[nameof(Port)] = value; }
    }

    public virtual bool Pooling
    {
      get { throw new NotImplementedException(); }
      set { throw new NotImplementedException(); }
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue(ConnectionStringKey, ConnectionStringBuilder.ConnectionString);
    }

    public override object Clone()
    {
      var copy = CreateInternal(ConnectionStringBuilder);
      copy.AccessConnectionString = this.AccessConnectionString;
      copy.ConnectionString = this.ConnectionString;
      return copy;
    }
  }
}
