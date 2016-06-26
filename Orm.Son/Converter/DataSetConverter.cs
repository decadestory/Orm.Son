using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Converter
{
    internal static class DataSetConverter
    {
        public static List<T> ToList<T>( this DataSet ds)
        {
            var table = ds.Tables[0];
            var list = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                var t = Activator.CreateInstance<T>();
                var propertypes = t.GetType().GetProperties();
                foreach (var pro in propertypes)
                {
                    var tempName = pro.Name;
                    if (!table.Columns.Contains(tempName)) continue;
                    var value = row[tempName];
                    if (value is DBNull) value = null;
                    pro.SetValue(t, value, null);
                }
                list.Add(t);
            }
            return list;
        }
    }
}
