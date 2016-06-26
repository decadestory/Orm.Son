using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orm.Son.Test.Connections;
using Orm.Son.Core;
using Orm.Son.Test.Entities;

namespace Orm.Son.Test
{
    [TestClass]
    public class MapperTest
    {
        [TestMethod]
        public void MapTest()
        {
            using (var db = new TestConnection())
            {
                var isok = db.CreateTable<Product>();
            }
        }
    }
}
