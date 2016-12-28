using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orm.Son.Test.Connections;
using Orm.Son.Core;
using Orm.Son.Test.Entities;
using Orm.Son.Mapper;

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

        [TestMethod]
        public void EntityMapTest()
        {
            var obj = new Demo
            {
                Name = "JerryDemoMany1",
                Age = 10,
                Score = 56,
                AddTime = DateTime.Now
            };

            var res = EntityMapper.Mapper<Demo, Demo3>(obj);

            Assert.IsNotNull(res);

        }




    }
}
