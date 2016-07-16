using Orm.Son.Converter;
using Orm.Son.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Core
{
    public static class MssqlOperator
    {
        public static int Insert<T>(this IDbConnection dbConn, T column)
        {
            var sql = column.InsertSql();
            var result = sql.ExeSqlWithParams(dbConn);
            return Convert.ToInt32(result);
        }

        public static bool Insert<T>(this IDbConnection dbConn, List<T> columns)
        {
            try
            {
                columns.ForEach(t => dbConn.Insert(t));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int Delete<T>(this IDbConnection dbConn, object id)
        {
            var sql = default(T).DeleteSql(id);
            var result = sql.ExeSqlWithParams(dbConn);
            return Convert.ToInt32(result);
        }

        public static int Delete<T>(this IDbConnection dbConn, Expression<Func<T, bool>> func)
        {
            var sql = default(T).DeleteSql(func);
            var result = sql.ExeSqlWithParams(dbConn);
            return Convert.ToInt32(result);
        }

        public static int Update<T>(this IDbConnection dbConn, T column)
        {
            var sql = column.UpdateSql();
            var result = sql.ExeSqlWithParams(dbConn);
            return Convert.ToInt32(result);
        }

        public static T Find<T>(this IDbConnection dbConn, object id)
        {
            var sql = default(T).SelectSql(id);
            var data = sql.ExeQueryWithParams(dbConn);
            var result = data.ToList<T>().FirstOrDefault();
            return result;
        }

        public static List<T> FindMany<T>(this IDbConnection dbConn, Expression<Func<T, bool>> func)
        {
            var sql = default(T).SelectSql(func);
            var data = sql.ExeQueryWithParams(dbConn);
            var result = data.ToList<T>();
            return result;
        }

        public static object ExecuteSql(this IDbConnection dbConn, string sql, List<SqlParameter> param = null)
        {
            var data = param == null ? sql.ExeSql(dbConn)
                 : new Tuple<string, List<SqlParameter>>(sql, param).ExeSqlWithParams(dbConn);
            return data;
        }

        public static List<T> ExecuteQuery<T>(this IDbConnection dbConn, string sql, List<SqlParameter> param = null)
        {
            var data = param == null ? sql.ExeQuery(dbConn)
                : new Tuple<string, List<SqlParameter>>(sql, param).ExeQueryWithParams(dbConn);
            var result = data.ToList<T>();
            return result;
        }

        public static bool CreateTable<T>(this IDbConnection dbConn)
        {
            try
            {
                var sql = default(T).CreateSql();
                var result = sql.ExeSql(dbConn);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DropTable<T>(this IDbConnection dbConn)
        {
            try
            {
                var sql = default(T).DropSql();
                var result = sql.ExeSql(dbConn);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
