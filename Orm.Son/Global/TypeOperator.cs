using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Global
{
    internal class TypeOperator
    {
        public static bool IsSqlString(Type t)
        {
            if (t == typeof(string) || t == typeof(DateTime) || t == typeof(bool)) return true;
            return false;
        }

        public static bool IsNullable(Type type)
        {
            if (type == typeof(string)) return true;
            return (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }
    }
}
