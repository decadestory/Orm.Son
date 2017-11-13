using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Core
{
    public class SonConnection : IDbConnection
    {
        public string ConnectionString { get; set; }
        public int ConnectionTimeout { get; set; }
        public string Database { get; set; }
        public ConnectionState State { get; }
        public SqlConnection DbConnection { set; get; }
        public SqlTransaction DbTransaction { set; get; }
        public SonConnection(string connectionString)
        {
            ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            DbConnection = new SqlConnection(ConnectionString);
            Database = DbConnection.Database;
            //DbConnection.Open();
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
