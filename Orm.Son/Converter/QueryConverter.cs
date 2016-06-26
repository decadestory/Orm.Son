using Orm.Son.Global;
using Orm.Son.Linq;
using Orm.Son.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Converter
{
    internal static class QueryConverter
    {
        public static string InsertSql<T>(this T entity)
        {
            var types = entity.GetType();
            var infos = types.GetProperties();
            var tableAttr = types.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : types.Name;
            var names = new List<string>();
            var values = new List<string>();
            foreach (var item in infos)
            {
                if (item.Name.Equals("Id")) continue;
                var val = item.GetValue(entity, null);
                if (item.PropertyType == typeof(DateTime) && (DateTime)val == default(DateTime)) continue;
                names.Add(item.Name);
                if (TypeOperator.IsSqlString(item.PropertyType))
                    values.Add("'" + val.ToString() + "'");
                else
                    values.Add(val.ToString());
            }
            return string.Format("INSERT INTO {0}({1}) VALUES({2});SELECT @@IDENTITY;", tableName, string.Join(",", names), string.Join(",", values));
        }

        public static string DeleteSql<T>(this T entity, object id)
        {
            var et = typeof(T);
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : et.Name;
            return string.Format("DELETE {0} WHERE ID={1}; SELECT @@ROWCOUNT;", tableName.ToUpper(), id);
        }

        public static string UpdateSql<T>(this T entity)
        {
            var types = entity.GetType();
            var infos = types.GetProperties();
            var tableAttr = types.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : types.Name;
            object id = null;
            var sets = new List<string>();
            foreach (var item in infos)
            {
                var val = item.GetValue(entity, null);

                if (item.Name.Equals("Id"))
                {
                    id = TypeOperator.IsSqlString(item.PropertyType) ? "'" + val + "'" : val;
                    continue;
                }

                if (TypeOperator.IsSqlString(item.PropertyType))
                    sets.Add(item.Name + "=" + "'" + val.ToString() + "'");
                else
                    sets.Add(item.Name + "=" + val.ToString());
            }
            var sql = string.Format("UPDATE {0} SET {1} WHERE ID={2};SELECT @@ROWCOUNT;", tableName, string.Join(",", sets), id.ToString());
            return sql;
        }

        public static string SelectSql<T>(this T entity, object id)
        {
            var et = typeof(T);
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : et.Name;
            return string.Format("SELECT * FROM {0} WHERE ID={1};", tableName, id);
        }

        public static string SelectSql<T>(this T entity, Expression<Func<T, bool>> func)
        {
            var condition = ExpressionResolve.Resolve(func);
            var et = typeof(T);
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name : et.Name;

            return string.Format("SELECT * FROM {0} WHERE {1};", tableName, condition);
        }

        public static string CreateSql<T>(this T entity)
        {
            var et = typeof(T);
            var cols = new List<string>();
            var infos = et.GetProperties();
            var tableAttr = et.GetCustomAttributes(typeof(TableNameAttribute), true);
            var tableName = tableAttr.Any() ? (tableAttr[0] as TableNameAttribute).Name: et.Name;

            var descriptions = new StringBuilder();

            var sql = "CREATE TABLE [{0}] ({1});";
            foreach (var item in infos)
            {
                var identity = item.Name == "Id" ? "IDENTITY(1,1)" : string.Empty;
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
