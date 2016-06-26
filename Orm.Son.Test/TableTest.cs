using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Orm.Son.Test.Entities;
using Orm.Son.Linq;
using Orm.Son.Test.Connections;
using Orm.Son.Core;

namespace Orm.Son.Test
{
    [TestClass]
    public class TableTest
    {
        [TestMethod]
        public void DataBase()
        {
            using (var db = new TestConnection())
            {
                var isCreated = db.CreateTable<User>();
                //var isDropped = db.DropTable<User>();
            }
        }
    }
}
