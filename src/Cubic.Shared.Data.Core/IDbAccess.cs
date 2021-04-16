namespace Cubic.Shared.Data.Core
{
  internal interface IDbAccess
  {
    string ConnectionString { get; set; }

    int Timeout { get; set; }

    string ApplicationName { get; set; }

    string Client { get; set; }

    int EnhancedTimeout { get; set; }

    System.Data.CommandBehavior DefaultCommandBehavior { get; set; }

    string Database { get; set; }
    string Server { get; set; }

    int Port { get; set; }

    bool Pooling { get; set; }

    bool IntegratedSecurity { get; set; }

    string UserId { get; set; }

    string Password { get; set; }
  }
}