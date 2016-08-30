using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orm.Son.Test.Connections;
using Orm.Son.Core;
using Orm.Son.Test.Entities;
using System.Data.SqlClient;
using System.Collections.Generic;

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
                    Name = "JerryDemoMany1",
                    Age = 10,
                    Score = 56,
                    AddTime = DateTime.Now
                };

                var obj2 = new Demo
                {
                    Name = "JerryDemoMany2",
                    Age = 10,
                    Score = 56,
                    AddTime = DateTime.Now
                };

                var pages = db.FindPage<Demo>(t => t.Age == 21, t => t.Score, 2, 100);

                var topResult = db.Top<Demo>(t => t.Age == 32, t => t.Score);
                var topResult2 = db.Top<Demo>(t => t.Age == 32, t => t.Score, true);


                var list = new List<Demo> { obj, obj2 };

                var res = db.Insert(obj);
                var res2 = db.Insert(list);
                db.Delete<Demo>(1010082);
                db.Delete<Demo>(t => t.Name == "JerryDemoMany1");
                db.Update(obj);

                var str = "Jerry60";
                var str2 = "329";
                var vr = db.FindMany<Demo>(t => t.Name == str);
                var vr2 = db.FindMany<Demo>(t => t.Name.Contains(str2));

                var val = 63;
                var b2 = db.FindMany<Demo>(t => t.Age == val);


                var s = db.Find<Demo>(1010000);
                var data = db.FindMany<Demo>(t => t.Age < 50 && t.IsDel == false);
                var data1 = db.FindMany<Demo>(t => t.Age.Equals(25));
                var data2 = db.FindMany<Demo>(t => t.Name.EndsWith("erry56471"));
                var data22 = db.FindMany<Demo>(t => t.Name.Contains("erry5"));

                var data3 = db.ExecuteQuery<Demo>("SELECT * FROM DEMO WHERE Name like '%erry56471';");
                var param33 = new List<SqlParameter> { new SqlParameter("@Name", "erry56471") };
                var data33 = db.ExecuteQuery<Demo>("SELECT * FROM DEMO WHERE Name like '%'+@Name;", param33);

                var data4 = db.ExecuteSql("UPDATE DEMO SET Name='JerryDemo',Age=10,Score=56,AddTime='2016/6/26 12:41:09',IsDel='True' WHERE ID = 1010000; SELECT @@ROWCOUNT;");
                var param44 = new List<SqlParameter> { new SqlParameter("@AddTime", "2016/6/26 12:41:09"), new SqlParameter("@IsDel", true) };
                var data44 = db.ExecuteQuery<Demo>("UPDATE DEMO SET Name='JerryDemo',Age=10,Score=56,AddTime=@AddTime,IsDel=@IsDel WHERE ID = 1010000; SELECT @@ROWCOUNT;", param44);

                var data5 = db.ExecuteSql("INSERT INTO DEMO(Name,Age,Score,AddTime,IsDel) VALUES('JerryDemo',10,56,'2016/6/26 14:04:43','False');SELECT @@IDENTITY;");
                var data6 = db.ExecuteSql(" SELECT count(1) FROM DEMO WHERE Age=25");

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
                var p = new Product { Name = "产品2", ProductLine = 12, Enable = false };
                var res = db.Insert(p);

                var cur = db.Find<Product>(res);
                cur.Enable = true;
                cur.Name = "修改后的产品";
                cur.ProductLine = 100;
                var result = db.Update(cur);

            }
        }
    }
}
