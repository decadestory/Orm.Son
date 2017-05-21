using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Converter
{
    internal static class TypeConverter
    {
        public static string ToSqlType(this Type type)
        {
            if (type == typeof(bool) || type == typeof(bool?))
                return "bit";
            if (type == typeof(byte) || type == typeof(byte?))
                return "tinyint";
            if (type == typeof(short) || type == typeof(short?))
                return "smallint";
            if (type == typeof(int) || type == typeof(int?))
                return "int";
            if (type == typeof(long) || type == typeof(long?))
                return "bigint";
            if (type == typeof(float) || type == typeof(float?))
                return "real";
            if (type == typeof(double) || type == typeof(double?))
                return "float";
            if (type == typeof(decimal) | type == typeof(decimal?))
                return "money";
            if (type == typeof(DateTime) | type == typeof(DateTime?))
                return "datetime";
            if (type == typeof(string))
                return "nvarchar";
            if (type == typeof(Guid) | type == typeof(Guid?))
                return "uniqueidentifier";

            throw new Exception("类型不支持");
        }

        public static SqlDbType ToDbType(Type type)
        {
            var typeMap = new Dictionary<Type, SqlDbType>();

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);

            typeMap[typeof(string)] = SqlDbType.NVarChar;
            typeMap[typeof(char[])] = SqlDbType.NVarChar;
            typeMap[typeof(int)] = SqlDbType.Int;
            typeMap[typeof(Int32)] = SqlDbType.Int;
            typeMap[typeof(Int16)] = SqlDbType.SmallInt;
            typeMap[typeof(Int64)] = SqlDbType.BigInt;
            typeMap[typeof(Byte[])] = SqlDbType.VarBinary;
            typeMap[typeof(Boolean)] = SqlDbType.Bit;
            typeMap[typeof(DateTime)] = SqlDbType.DateTime2;
            typeMap[typeof(DateTimeOffset)] = SqlDbType.DateTimeOffset;
            typeMap[typeof(Decimal)] = SqlDbType.Decimal;
            typeMap[typeof(Double)] = SqlDbType.Float;
            typeMap[typeof(Decimal)] = SqlDbType.Money;
            typeMap[typeof(Byte)] = SqlDbType.TinyInt;
            typeMap[typeof(TimeSpan)] = SqlDbType.Time;

            return typeMap[(type)];
        }

        public static string OperatorConverter(ExpressionType expressiontype)
        {
            switch (expressiontype)
            {
                case ExpressionType.And:
                    return "and";
                case ExpressionType.AndAlso:
                    return "and";
                case ExpressionType.Or:
                    return "or";
                case ExpressionType.OrElse:
                    return "or";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.NotEqual:
                    return "<>";
                default:
                    throw new Exception(string.Format("不支持{0}此种运算符查找！" + expressiontype));
            }
        }


    }
}
