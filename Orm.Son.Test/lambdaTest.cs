using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orm.Son.Linq;
using Orm.Son.Test.Entities;

namespace Orm.Son.Test
{
    [TestClass]
    public class lambdaTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a = "hello";
            var b = "hello2";
            var d = DateTime.Now;
            var res = ExpressionResolve.Resolve<Demo>(t => t.Name == "hello" && (t.Name == a || t.Name == b) || t.IsDel==true && t.AddTime<d || t.Name =="jerry" || t.Name == "56");
            var res2 = ExpressionResolve.Resolve<Demo>(t=>t.Name=="Hello" || (t.Name.Contains( "He") && t.Age > 20));
            var res3 = ExpressionResolve.Resolve<Demo>(t=> t.Name.Contains("He") && (t.Age > 20 || t.AddTime < d));
            var res4 = ExpressionResolve.Resolve<Demo>(t=> (t.Age > 20 || t.AddTime < d)  && t.Name.Contains("He"));
            var res5 = ExpressionResolve.Resolve<Demo>(t=> (t.Age > 20 && t.AddTime < d)  || (t.Name.Contains("Hel") && t.Name.Contains("He")));
            Assert.IsTrue(true);

        }
    }
}
