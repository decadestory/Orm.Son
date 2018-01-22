using Orm.Son.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orm.Son.Core.CCore;

namespace Orm.Son.Core.Test.Connections
{
    public class TestConnection : SonConnection
    {
        public TestConnection() : base("conn")
        {
        }
    }
}
