using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orm.Son.Core;
using Orm.Son.Test.Entities;
using System.Threading.Tasks;

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

        [TestMethod]
        public void TestMethod2()
        {
           for(var i=0; i<100;i++)
            {
                var obj2 = new Demo
                {
                    Name = "JerryDemoMany2",
                    Age = 10,
                    Score = 56,
                    AddTime = DateTime.Now
                };
                SonFact.Cur.Insert(obj2);
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestMethod3()
        {
            Parallel.For(0,10000,t=> {
                var obj2 = new Demo
                {
                    Name = "JerryDemoMany2",
                    Age = 10,
                    Score = 56,
                    AddTime = DateTime.Now
                };
                SonFact.Cur.Insert(obj2);
            });

            Assert.IsTrue(true);
        }

    }
}
