using Orm.Son.Global;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Linq
{
    public class ExpressionResolve
    {
        public static Tuple<string, List<SqlParameter>> Resolve<T>(Expression<Func<T, bool>> func)
        {
            var condition = ResovleFunc(func.Body);
            return condition;
        }

        private static Tuple<string, List<SqlParameter>> ResovleFunc(Expression express)
        {
            var inner = express as BinaryExpression;

            if (express.NodeType == ExpressionType.Call)
            {
                var res = ResovleLinq(express);
                return new Tuple<string, List<SqlParameter>>(res.Item1,new List<SqlParameter> { res.Item2});
            }

            if (inner.Left.NodeType == ExpressionType.MemberAccess)
            {
                var res = ResovleFuncRight(express);
                return new Tuple<string, List<SqlParameter>>(res.Item1, new List<SqlParameter> { res.Item2 });
            }

            var cur = ResovleFunc(inner.Left);
            var resovled = inner.Right.NodeType == ExpressionType.Call ? ResovleLinq(inner.Right) : ResovleFuncRight(inner.Right);
            var opr = OperatorConverter(inner.NodeType);
            var reslist = cur.Item2;
            reslist.Add(resovled.Item2);
            return new Tuple<string, List<SqlParameter>>(cur.Item1 + " " + opr + " " + resovled.Item1, reslist);
        }

        private static Tuple<string, SqlParameter> ResovleFuncRight(Expression express)
        {
            var inner = express as BinaryExpression;
            if (inner == null) return new Tuple<string, SqlParameter>(string.Empty, null);
            var sl = (inner.Left as MemberExpression).Member.Name;
            var sr = (inner.Right as ConstantExpression).Value;
            var srt = (inner.Right as ConstantExpression).Type;
            var op = OperatorConverter(inner.NodeType);
            return new Tuple<string, SqlParameter>(sl + op + "@" + sl, new SqlParameter("@" + sl, sr));
        }

        public static Tuple<string, SqlParameter> ResovleLinq(Expression expression)
        {
            var MethodCall = expression as MethodCallExpression;
            var MethodName = MethodCall.Method.Name;
            if (MethodName == "Contains")
            {
                object value = (MethodCall.Arguments[0] as ConstantExpression).Value;
                var name = (MethodCall.Object as MemberExpression).Member.Name;
                var cd = string.Format("{0} like '%'+@{0}+'%'", name);
                return new Tuple<string, SqlParameter>(cd, new SqlParameter("@" + name, value));
            }

            if (MethodName == "Equals")
            {
                var name = (MethodCall.Object as MemberExpression).Member.Name;
                object value = (MethodCall.Arguments[0] as ConstantExpression).Value;
                Type valeType = (MethodCall.Arguments[0] as ConstantExpression).Type;
                var cd = string.Format("{0} = @{0}", name);
                return new Tuple<string, SqlParameter>(cd, new SqlParameter("@" + name, value));
            }

            if (MethodName == "EndsWith")
            {
                object value = (MethodCall.Arguments[0] as ConstantExpression).Value;
                var name = (MethodCall.Object as MemberExpression).Member.Name;
                var cd = string.Format("{0} like '%'+@{0}", name);
                return new Tuple<string, SqlParameter>(cd, new SqlParameter("@" + name, value));
            }
            return new Tuple<string, SqlParameter>(string.Empty, null);
        }

        private static string OperatorConverter(ExpressionType expressiontype)
        {
            switch (expressiontype)
            {
                case ExpressionType.And:
                    return "and";
                case ExpressionType.AndAlso:
                    return "and";
                case ExpressionType.Or:
                    return "or";
                case ExpressionType.OrElse:
                    return "or";
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.NotEqual:
                    return "<>";
                default:
                    throw new Exception(string.Format("不支持{0}此种运算符查找！" + expressiontype));
            }
        }
    }
}
