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
        #region 直接执行

        public static object ExeSql(this string sql, IDbConnection dbConn)
        {
            try
            {
                dbConn.Open();
                var dbCommand = dbConn.CreateCommand();
                dbCommand.CommandText = sql;
                var result = dbCommand.ExecuteScalar();
                return result;
            }
            finally
            {
                dbConn.Close();
            }

        }

        public static DataSet ExeQuery(this string sql, IDbConnection dbConn)
        {
            var dbCommand = dbConn.CreateCommand();
            dbCommand.CommandText = sql;

            var ds = new DataSet();
            var sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = (SqlCommand)dbCommand;
            sqlDataAdapter.Fill(ds);
            return ds;
        }

        #endregion

        #region 参数化方式执行

        public static object ExeSqlWithParams(this Tuple<string, List<SqlParameter>> sql, IDbConnection dbConn)
        {
            try
            {
                dbConn.Open();
                var dbCommand = dbConn.CreateCommand();
                dbCommand.CommandText = sql.Item1;
                sql.Item2.ForEach(t => dbCommand.Parameters.Add(t));
                var result = dbCommand.ExecuteScalar();
                return result;
            }
            finally
            {
                dbConn.Close();
            }

        }

        public static DataSet ExeQueryWithParams(this Tuple<string, List<SqlParameter>> sql, IDbConnection dbConn)
        {
            var dbCommand = dbConn.CreateCommand();
            dbCommand.CommandText = sql.Item1;
            var sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = (SqlCommand)dbCommand;
            sql.Item2.ForEach(t => sqlDataAdapter.SelectCommand.Parameters.Add(t));
            var ds = new DataSet();
            sqlDataAdapter.Fill(ds);
            return ds;
        }

        public static Tuple<DataSet, object> ExeSqlWithParamsPage(this Tuple<string, List<SqlParameter>, string> sql, IDbConnection dbConn)
        {
            try
            {
                dbConn.Open();
                var dbCommand = dbConn.CreateCommand();
                sql.Item2.ForEach(t => dbCommand.Parameters.Add(t));

                dbCommand.CommandText = sql.Item1;
                var sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.SelectCommand = (SqlCommand)dbCommand;
                var ds = new DataSet();
                var resultData = sqlDataAdapter.Fill(ds);

                dbCommand.CommandText = sql.Item3;
                var resultTotal = dbCommand.ExecuteScalar();
                return new Tuple<DataSet, object>(ds, resultTotal);
            }
            finally
            {

                dbConn.Close();
            }

        }

        #endregion
    }
}
