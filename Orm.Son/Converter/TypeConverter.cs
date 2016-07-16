using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
