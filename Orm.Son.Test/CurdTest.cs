using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orm.Son.Test.Connections;
using Orm.Son.Core;
using Orm.Son.Test.Entities;

namespace Orm.Son.Test
{
    [TestClass]
    public class CurdTest
    {
        [TestMethod]
        public void Curd()
        {
            using (var db = new TestConnection())
            {
                var obj = new Demo
                {
                    Id = 1010082,
                    Name = "JerryDemo",
                    Age = 10,
                    Score = 56,
                    AddTime = DateTime.Now
                };

                //var res = db.Insert(obj);
                //db.Delete<Demo>(1010082);
                //db.Update(obj);
                //db.Find<Demo>(1010000);
                //var data = db.FindMany<Demo>(t =>t.Age.Equals(25));
                //var data2 = db.FindMany<Demo>(t =>t.Name.EndsWith("erry56471"));
                //var data3 = db.ExecuteQuery<Demo>("SELECT * FROM DEMO WHERE Name like '%erry56471';");
                //var data4 = db.ExecuteSql("UPDATE DEMO SET Name='JerryDemo',Age=10,Score=56,AddTime='2016/6/26 12:41:09',IsDel='True' WHERE ID = 1010000; SELECT @@ROWCOUNT;");
                //var data5 = db.ExecuteSql("INSERT INTO DEMO(Name,Age,Score,AddTime,IsDel) VALUES('JerryDemo',10,56,'2016/6/26 14:04:43','False');SELECT @@IDENTITY;");
                //var data6 = db.ExecuteSql(" SELECT count(1) FROM DEMO WHERE Age=25");
            }
        }

        [TestMethod]
        public void TransactionTest()
        {
            using (var db = new TestConnection())
            {
                var tran = db.BeginTransaction();
                var obj = new Demo
                {
                    Id = 1010082,
                    Name = "JerryDemo",
                    Age = 10,
                    Score = 56,
                    AddTime = DateTime.Now
                };

                try
                {
                    var res = db.Insert(obj);
                    db.ExecuteSql("UPDATE DEMO SET Name='JerryDemo',Age=10,Score=56s,AddTime='2016/6/26 12:41:09',IsDel='True' WHERE ID = 1010000; SELECT @@ROWCOUNT;");
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }


            }
        }

        [TestMethod]
        public void MappedCurd()
        {
            using (var db = new TestConnection())
            {
                //var p = new Product { Name = "产品2", ProductLine = 12, Enable = false };
                //var res = db.Insert(p);

                var cur = db.Find<Product>(3);
                cur.Enable = true;
                cur.Name = "修改后的产品";
                cur.ProductLine = 100;
                var res = db.Update(cur);
                
            }
        }
    }
}
