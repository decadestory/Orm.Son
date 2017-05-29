using Orm.Son.Global;
using Orm.Son.Linq;
using Orm.Son.Mapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Orm.Son.Converter
{
    internal static class QueryConverter
    {
        public static Tuple<string, List<SqlParameter>> InsertSql<T>(this T entity)
        {
            var types = entity.GetType();
            var infos = types.GetProperties();

            var tableAttr = types.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : types.Name;
            var names = new List<string>();
            var values = new List<string>();
            var sqlParamsVal = new List<SqlParameter>();
            var keyInfo = infos.FirstOrDefault(t => (t.GetCustomAttributes(typeof(KeyAttribute)).Any()));
            var keyStr = keyInfo != null ? keyInfo.Name : "Id";

            foreach (var item in infos)
            {
                if (item.Name.Equals(keyStr)) continue;
                var val = item.GetValue(entity, null);
                var dbType = TypeConverter.ToDbType(item.PropertyType);
                var isNullable = item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

                if (item.PropertyType == typeof(DateTime) && (DateTime)val == default(DateTime)) continue;
                if (val == null) continue;
                names.Add("[" + item.Name + "]");
                sqlParamsVal.Add(new SqlParameter("@" + item.Name, dbType) { SqlValue = val });
                values.Add("@" + item.Name);
            }
            var sql = string.Format("INSERT INTO [{0}]({1}) VALUES({2});SELECT @@IDENTITY;", tableName, string.Join(",", names), string.Join(",", values));
            return new Tuple<string, List<SqlParameter>>(sql, sqlParamsVal);
        }

        public static Tuple<string, List<SqlParameter>> DeleteSql<T>(this T entity, object id)
        {
            var et = typeof(T);
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : et.Name;
            var keyInfo = et.GetProperties().FirstOrDefault(t => (t.GetCustomAttributes(typeof(KeyAttribute)).Any()));
            var keyStr = keyInfo != null ? keyInfo.Name : "ID";

            var sqlParamsVal = new List<SqlParameter> {
                new SqlParameter("@Id",id)
            };

            var sql = string.Format("DELETE [{0}] WHERE [{1}]=@Id; SELECT @@ROWCOUNT;", tableName.ToUpper(), keyStr);
            return new Tuple<string, List<SqlParameter>>(sql, sqlParamsVal);
        }

        public static string DeleteSql<T>(this T entity, List<string> ids)
        {
            var et = typeof(T);
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : et.Name;
            var keyInfo = et.GetProperties().FirstOrDefault(t => (t.GetCustomAttributes(typeof(KeyAttribute)).Any()));
            var keyStr = keyInfo != null ? keyInfo.Name : "ID";

            var newIds =new  List<string>();
            foreach (var id in ids)
            {
                var reg = new Regex(@"^[-A-Za-z0-9]+$") ;
                if (reg.IsMatch(id + "")) newIds.Add(id + "");
            }

            var idstr ="'" + string.Join("','", newIds) +"'";
            var sql = string.Format("DELETE [{0}] WHERE [{1}] in ({2}); SELECT @@ROWCOUNT;", tableName.ToUpper(), keyStr, idstr);
            return sql;
        }

        public static string DeleteSql<T>(this T entity, List<int> ids)
        {
            var et = typeof(T);
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : et.Name;
            var keyInfo = et.GetProperties().FirstOrDefault(t => (t.GetCustomAttributes(typeof(KeyAttribute)).Any()));
            var keyStr = keyInfo != null ? keyInfo.Name : "ID";

            var newIds = new List<string>();
            foreach (var id in ids)
            {
                var reg = new Regex(@"^[-A-Za-z0-9]+$");
                if (reg.IsMatch(id + "")) newIds.Add(id + "");
            }

            var idstr = "'" + string.Join("','", newIds) + "'";
            var sql = string.Format("DELETE [{0}] WHERE [{1}] in ({2}); SELECT @@ROWCOUNT;", tableName.ToUpper(), keyStr, idstr);
            return sql;
        }

        public static Tuple<string, List<SqlParameter>> DeleteSql<T>(this T entity, Expression<Func<T, bool>> func)
        {
            var condition = ExpressionResolve.Resolve(func);
            var et = typeof(T);
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : et.Name;
            var sql = string.Format("DELETE [{0}] WHERE {1}; SELECT @@ROWCOUNT;", tableName.ToUpper(), condition.Item1);
            return new Tuple<string, List<SqlParameter>>(sql, condition.Item2);
        }

        public static Tuple<string, List<SqlParameter>> UpdateSql<T>(this T entity)
        {
            var types = entity.GetType();
            var infos = types.GetProperties();
            var tableAttr = types.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : types.Name;
            var sqlParamsVal = new List<SqlParameter>();
            var sets = new List<string>();
            var id = string.Empty;
            var keyInfo = infos.FirstOrDefault(t => (t.GetCustomAttributes(typeof(KeyAttribute)).Any()));
            var keyStr = keyInfo != null ? keyInfo.Name : "Id";

            foreach (var item in infos)
            {
                var val = item.GetValue(entity, null);
                var dbType = TypeConverter.ToDbType(item.PropertyType);
                var isNullable = item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

                if (item.Name.Equals(keyStr))
                {
                    sqlParamsVal.Add(new SqlParameter("@Id", dbType) { SqlValue = val });
                    id = val.ToString();
                    continue;
                }
                sets.Add("[" + item.Name + "]" + "=@" + item.Name);
                sqlParamsVal.Add(new SqlParameter("@" + item.Name, dbType) { SqlValue = val == null ? DBNull.Value : val, IsNullable = isNullable });
            }

            var sql = string.Format("UPDATE [{0}] SET {1} WHERE [{2}]=@Id;SELECT @@ROWCOUNT;", tableName, string.Join(",", sets), keyStr);
            return new Tuple<string, List<SqlParameter>>(sql, sqlParamsVal);
        }

        public static Tuple<string, List<SqlParameter>> SelectSql<T>(this T entity, object id)
        {
            var et = typeof(T);
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : et.Name;
            var sqlParamsVal = new List<SqlParameter> { new SqlParameter("@Id", id) };
            var keyInfo = et.GetProperties().FirstOrDefault(t => (t.GetCustomAttributes(typeof(KeyAttribute)).Any()));
            var keyStr = keyInfo != null ? keyInfo.Name : "ID";


            var sql = string.Format("SELECT * FROM [{0}] WITH(NOLOCK) WHERE [{1}]=@Id;", tableName, keyStr);
            return new Tuple<string, List<SqlParameter>>(sql, sqlParamsVal);
        }

        public static Tuple<string, List<SqlParameter>> SelectSql<T>(this T entity, Expression<Func<T, bool>> func, Expression<Func<T, object>> order = null, bool isDesc = false)
        {
            var condition = ExpressionResolve.Resolve(func);
            var orderName = order == null ? "" : ExpressionResolve.ResolveSingle(order);
            var sortMode = isDesc ? "DESC" : "ASC";
            var et = typeof(T);
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : et.Name;

            var sql = order == null
               ? string.Format("SELECT * FROM [{0}] WITH(NOLOCK) WHERE {1} ;", tableName, condition.Item1)
               : string.Format("SELECT * FROM [{0}] WITH(NOLOCK) WHERE {1} ORDER BY {2} {3};", tableName, condition.Item1, orderName, sortMode);
            return new Tuple<string, List<SqlParameter>>(sql, condition.Item2);
        }

        public static Tuple<string, List<SqlParameter>> TopSql<T>(this T entity, Expression<Func<T, bool>> func, Expression<Func<T, object>> order, bool isDesc = false)
        {
            var condition = ExpressionResolve.Resolve(func);
            var orderName = order == null ? "" : ExpressionResolve.ResolveSingle(order);
            var sortMode = isDesc ? "DESC" : "ASC";
            var et = typeof(T);
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : et.Name;
            var sql = order == null
                ? string.Format("SELECT TOP 1 * FROM [{0}] WITH(NOLOCK) WHERE {1} ;", tableName, condition.Item1)
                : string.Format("SELECT TOP 1 * FROM [{0}] WITH(NOLOCK) WHERE {1} ORDER BY [{2}] {3};", tableName, condition.Item1, orderName, sortMode);
            return new Tuple<string, List<SqlParameter>>(sql, condition.Item2);
        }

        public static Tuple<string, List<SqlParameter>, string> PageSql<T>(this T entity, Expression<Func<T, bool>> where, Expression<Func<T, object>> order, int page, int limit, bool isDesc = false)
        {
            var condition = ExpressionResolve.Resolve(where);
            var orderName = ExpressionResolve.ResolveSingle(order);
            var sortMode = isDesc ? "DESC" : "ASC";
            var et = typeof(T);
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : et.Name;
            var sql = string.Format(@"WITH PAGERESULT AS (SELECT ROW_NUMBER() OVER(ORDER BY [{0}] {1}) AS NUMBER, * FROM [{2}] WHERE {3}) "
                                    , orderName, sortMode, tableName, condition.Item1);
            var sqlData = string.Format("{0} SELECT TOP {1} * FROM PAGERESULT WHERE NUMBER>{2} ;", sql, limit, (page - 1) * limit);
            var sqlCnt = string.Format("{0} SELECT COUNT(1) FROM PAGERESULT ;", sql);

            return new Tuple<string, List<SqlParameter>, string>(sqlData, condition.Item2, sqlCnt);
        }

        public static string CreateSql<T>(this T entity)
        {
            var et = typeof(T);
            var cols = new List<string>();
            var infos = et.GetProperties();
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : et.Name;

            var descriptions = new StringBuilder();

            var sql = "CREATE TABLE [{0}] ({1});";
            var pk = "CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED([{1}] ASC)";
            var isHasPrimary = false;

            foreach (var item in infos)
            {
                var identity = string.Empty;
                if (item.Name == "Id" || item.GetCustomAttributes(typeof(KeyAttribute), true).Any())
                {
                    identity = "IDENTITY(1,1)";
                    pk = string.Format(pk, tableName, item.Name);
                    isHasPrimary = true;
                }

                var nullable = TypeOperator.IsNullable(item.PropertyType) ? "NULL" : "NOT NULL";
                var timeDefault = item.PropertyType == typeof(DateTime) ? "DEFAULT (GETDATE())" : string.Empty;
                var sqlType = item.PropertyType.ToSqlType();
                sqlType = sqlType == "nvarchar" ? "[nvarchar](max)" : string.Format("[{0}]", sqlType);
                cols.Add(string.Format("[{0}] {1} {2} {3} {4}", item.Name, sqlType, identity, nullable, timeDefault));

                var colAttr = item.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (colAttr.Any())
                {
                    var attrName = (colAttr[0] as DescriptionAttribute).Name;
                    descriptions.AppendLine(string.Format(@"EXEC sp_addextendedproperty @name=N'MS_Description',@value=N'{0}',@level0type=N'Schema',@level0name=N'dbo',@level1type=N'Table',@level1name=N'{1}',@level2type=N'Column',@level2name=N'{2}';", attrName, tableName, item.Name));
                }
            }
            if (isHasPrimary) cols.Add(pk);
            var result = string.Format(sql + "{2}", tableName, string.Join(",", cols), descriptions);
            return result;
        }

        public static string DropSql<T>(this T entity)
        {
            var et = typeof(T);
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : et.Name;
            return string.Format("DROP TABLE [{0}]", tableName);
        }

    }
}
