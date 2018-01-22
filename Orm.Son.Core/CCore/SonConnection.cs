using System.Data;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Orm.Son.Core.CCore
{
    public class SonConnection : IDbConnection
    {
        public IConfigurationRoot configuration { get; set; }
        public string ConnectionString { get; set; }
        public int ConnectionTimeout { get; set; }
        public string Database { get; set; }
        public ConnectionState State { get; }
        public IDbConnection DbConnection { set; get; }
        public IDbTransaction DbTransaction { set; get; }
        public SonConnection(string connectionString)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            configuration = builder.Build();
            ConnectionString = configuration.GetConnectionString(connectionString);
            DbConnection = new System.Data.SqlClient.SqlConnection(ConnectionString);
            Database = DbConnection.Database;
        }

        public IDbTransaction BeginTransaction()
        {
            DbTransaction = DbConnection.BeginTransaction();
            return DbTransaction;
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return DbConnection.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName)
        {
            DbConnection.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            DbConnection.Close();
        }

        public IDbCommand CreateCommand()
        {
            var cmd = DbConnection.CreateCommand();
            if (DbTransaction != null) cmd.Transaction = DbTransaction;
            return cmd;
        }

        public void Dispose()
        {
            DbConnection.Close();
            DbConnection.Dispose();
        }

        public void Open()
        {
            DbConnection.Open();
        }
    }
}
