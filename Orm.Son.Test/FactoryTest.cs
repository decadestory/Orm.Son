using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orm.Son.Core;
using Orm.Son.Test.Entities;

namespace Orm.Son.Test
{
    [TestClass]
    public class FactoryTest
    {
        [TestInitialize]
        public void Init()
        {
            SonFact.init("conn");
        }

        [TestMethod]
        public void TestMethod1()
        {
            var obj2 = new Demo
            {
                Name = "JerryDemoMany2",
                Age = 10,
                Score = 56,
                AddTime = DateTime.Now
            };

            var r = SonFact.Cur.Insert(obj2);
            SonFact.Cur.Insert(obj2);
            SonFact.Cur.Insert(obj2);
            SonFact.Cur.Insert(obj2);
            SonFact.Cur.Insert(obj2);
            SonFact.Cur.Insert(obj2);
            SonFact.Cur.Insert(obj2);

            Assert.IsTrue(true);
        }
    }
}
