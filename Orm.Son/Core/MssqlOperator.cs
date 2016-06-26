﻿using Orm.Son.Converter;
using Orm.Son.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
            var result = sql.ExeSql(dbConn);
            return Convert.ToInt32(result);
        }

        public static bool InsertMany<T>(this IDbConnection dbConn, List<T> columns)
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
            var result = sql.ExeSql(dbConn);
            return Convert.ToInt32(result);
        }

        public static int Update<T>(this IDbConnection dbConn, T column)
        {
            var sql = column.UpdateSql();
            var result = sql.ExeSql(dbConn);
            return Convert.ToInt32(result);
        }

        public static T Find<T>(this IDbConnection dbConn,object id)
        {
            var sql = default(T).SelectSql(id);
            var data = sql.ExeQuery(dbConn);
            var result = data.ToList<T>().FirstOrDefault();
            return result;
        }

        public static List<T> FindMany<T>(this IDbConnection dbConn, Expression<Func<T, bool>> func)
        {
            var sql = default(T).SelectSql(func);
            var data = sql.ExeQuery(dbConn);
            var result = data.ToList<T>();
            return result;
        }

        public static object ExecuteSql(this IDbConnection dbConn,string sql)
        {
            var data = sql.ExeSql(dbConn);
            return data;
        }

        public static List<T> ExecuteQuery<T>(this IDbConnection dbConn, string sql)
        {
            var data = sql.ExeQuery(dbConn);
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
