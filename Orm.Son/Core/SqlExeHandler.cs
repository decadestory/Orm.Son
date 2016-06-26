using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Core
{
    internal static class SqlExeHandler
    {
        public static object ExeSql(this string sql, IDbConnection dbConn)
        {
            var dbCommand = dbConn.CreateCommand();
            dbCommand.CommandText = sql;
            var result = dbCommand.ExecuteScalar();
            return result;
        }

        public static DataSet ExeQuery(this string sql, IDbConnection dbConn)
        {
            var dbCommand = dbConn.CreateCommand();
            dbCommand.CommandText = sql;

            var ds = new DataSet();
            var sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = (SqlCommand)dbCommand;
            var result = sqlDataAdapter.Fill(ds);
            return ds;
        }
    }
}
