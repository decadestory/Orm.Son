using Orm.Son.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Linq
{
    public class ExpressionResolve
    {
        public static string Resolve<T>(Expression<Func<T, bool>> func)
        {
            string condition = ResovleFunc(func.Body);
            return condition;
        }

        private static string ResovleFunc(Expression express)
        {
            var inner = express as BinaryExpression;

            if(express.NodeType == ExpressionType.Call)
                return ResovleLinq(express);

            if (inner.Left.NodeType == ExpressionType.MemberAccess)
                return ResovleFuncRight(express);

            var cur = ResovleFunc(inner.Left);
            var resovled = inner.Right.NodeType == ExpressionType.Call ? ResovleLinq(inner.Right) : ResovleFuncRight(inner.Right);
            var opr = OperatorConverter(inner.NodeType);
            return cur + " " + opr + " " + resovled;
        }

        private static string ResovleFuncRight(Expression express)
        {
            var inner = express as BinaryExpression;
            if (inner == null) return string.Empty;
            var sl = (inner.Left as MemberExpression).Member.Name;
            var sr = (inner.Right as ConstantExpression).Value;
            var srt = (inner.Right as ConstantExpression).Type;
            var op = OperatorConverter(inner.NodeType);
            if (TypeOperator.IsSqlString(srt)) sr = "'" + sr + "'";
            return sl + op + sr;
        }

        public static string ResovleLinq(Expression expression)
        {
            var MethodCall = expression as MethodCallExpression;
            var MethodName = MethodCall.Method.Name;
            if (MethodName == "Contains")
            {
                object temp_Vale = (MethodCall.Arguments[0] as ConstantExpression).Value;
                string value = string.Format("%{0}%", temp_Vale);
                string name = (MethodCall.Object as MemberExpression).Member.Name;
                string result = string.Format("{0} like '{1}'", name, value);
                return result;
            }

            if (MethodName == "Equals")
            {
                string name = (MethodCall.Object as MemberExpression).Member.Name;
                object temp_Vale = (MethodCall.Arguments[0] as ConstantExpression).Value;
                Type valeType= (MethodCall.Arguments[0] as ConstantExpression).Type;
                if (TypeOperator.IsSqlString(valeType)) temp_Vale = "'" + valeType + "'";
                string result = string.Format("{0} = {1}", name, temp_Vale);
                return result;
            }

            if (MethodName == "EndsWith")
            {
                object temp_Vale = (MethodCall.Arguments[0] as ConstantExpression).Value;
                string name = (MethodCall.Object as MemberExpression).Member.Name;
                string result = string.Format("{0} like '%{1}'", name, temp_Vale);
                return result;
            }
            return string.Empty;
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
