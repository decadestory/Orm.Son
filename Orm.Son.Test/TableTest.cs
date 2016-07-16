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
        public void DataBaseCreate()
        {
            using (var db = new TestConnection())
            {
                var isCreated = db.CreateTable<User>();
                var isCreated2 = db.CreateTable<User>();
                var obj = new User
                {
                    Name = "Jerry",
                    Age = 15,
                    Score = 99
                };
                var ss = db.Insert(obj);
            }
        }

        [TestMethod]
        public void DataBaseDrop()
        {
            using (var db = new TestConnection())
            {
                var isDropped = db.DropTable<User>();
            }
        }
    }
}
