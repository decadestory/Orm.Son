using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Mapper
{
    public static class EntityMapper
    {
        public static TOut Mapper<Tin, TOut>(Tin inObj)
        {
            var outObj = Activator.CreateInstance<TOut>();
            var OutType = typeof(TOut);
            var inObjs = typeof(Tin).GetProperties();
            foreach (var obj in inObjs)
            {
                var vIn = obj.GetValue(inObj);
                var pOut = OutType.GetProperty(obj.Name);
                if (pOut == null) continue;
                if (vIn == null) continue;

                var nuPout = Nullable.GetUnderlyingType(pOut.PropertyType);
                try
                {
                    var vOut = Convert.ChangeType(vIn, nuPout ?? pOut.PropertyType);
                    pOut.SetValue(outObj, vOut);
                }
                catch
                {
                    continue;
                }
            }
            return outObj;
        }
    }
}
