using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orm.Son.Core.Test.Connections;
using Orm.Son.Core.CCore;
using Orm.Son.Core.Test.Entities;
using System;

namespace Orm.Son.Core.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var db = new TestConnection();
            var createRes = db.CreateTable<User>();

            var user = new User
            {
                Name = "Jerry",
                Age = 13,
                AddTime = DateTime.Now,
                Score = 100
            };

            db.Insert(user);

            Assert.IsTrue(true);

        }
    }
}
