using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Core
{
    public static class SonFact
    {
        public static string connString = "conn";
        public static void init(string connStr)
        {
            connString = connStr;
        }

        public static SonConnection Cur
        {
            get
            {
                var cache = CallContext.GetData("SonConnectionCache") as SonConnection;
                if (cache != null) return cache;
                var sonConn = new SonConnection(connString);
                CallContext.SetData("SonConnectionCache", sonConn);
                return sonConn;
            }
        }

    }
}
