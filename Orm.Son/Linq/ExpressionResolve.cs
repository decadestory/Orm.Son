using Orm.Son.Converter;
using Orm.Son.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Orm.Son.Linq
{
    public class ExpressionResolve
    {
        public static string ResolveSingle<T>(Expression<Func<T, object>> func)
        {
            var body = func.Body as dynamic;
            return GetMemberName(body);
        }

        public static Tuple<string, List<SqlParameter>> Resolve<T>(Expression<Func<T, bool>> func)
        {
            var index = 0;
            var condition = ResovleFunc(func.Body, ref index);
            //System.Diagnostics.Debug.WriteLine(condition.Item1);
            return condition;
        }
         
        private static Tuple<string, List<SqlParameter>> ResovleFunc(Expression express, ref int index)
        {
            if (express.NodeType == ExpressionType.Call)
            {
                var res = ResovleLinq(express, ref index);
                return new Tuple<string, List<SqlParameter>>(res.Item1, new List<SqlParameter> { res.Item2 });
            }

            var inner = express as BinaryExpression;
            if (inner.Left.NodeType == ExpressionType.MemberAccess || inner.Left.NodeType == ExpressionType.Convert)
            {
                var res = ResovleFuncRight(express, ref index);
                return new Tuple<string, List<SqlParameter>>(res.Item1, new List<SqlParameter> { res.Item2 });
            }

            var curLeft = ResovleFunc(inner.Left, ref index);
            var curRight = ResovleFunc(inner.Right, ref index);

            var opr = TypeConverter.OperatorConverter(inner.NodeType);
            curLeft.Item2.AddRange(curRight.Item2);

            //System.Diagnostics.Debug.WriteLine(inner.NodeType.ToString());

            return inner.NodeType == ExpressionType.AndAlso //|| inner.NodeType == ExpressionType.OrElse
                ? new Tuple<string, List<SqlParameter>>("(" + curLeft.Item1 + ")" + " " + opr + " " + "(" + curRight.Item1 + ")", curLeft.Item2)
                : new Tuple<string, List<SqlParameter>>(curLeft.Item1 + " " + opr + " " + curRight.Item1, curLeft.Item2);
        }

        private static Tuple<string, SqlParameter> ResovleFuncRight(Expression express, ref int index)
        {
            index++;
            var inner = express as BinaryExpression;
            if (inner == null) return new Tuple<string, SqlParameter>(string.Empty, null);

            var sl = GetMemberName(inner.Left);
            var sr = GetMemberValue(inner.Right);
            var srt = inner.Right.Type;

            var dbType = TypeConverter.ToDbType(srt);
            var par = new SqlParameter("@" + sl + index, dbType) { SqlValue = sr };
            var op = TypeConverter.OperatorConverter(inner.NodeType);
            return new Tuple<string, SqlParameter>("[" + sl + "] " + op + "@" + sl + index, par);
        }

        public static Tuple<string, SqlParameter> ResovleLinq(Expression expression, ref int index)
        {
            index++;
            var MethodCall = expression as MethodCallExpression;
            var MethodName = MethodCall.Method.Name;
            var constantExp = MethodCall.Arguments[0] as ConstantExpression;

            if (MethodName == "Contains")
            {
                var name = (MethodCall.Object as MemberExpression).Member.Name;
                var value = constantExp == null
                ? GetValueOfMemberExpression((MemberExpression)MethodCall.Arguments[0])
                : constantExp.Value;

                var cd = string.Format("[{0}] like '%'+@{1}+'%'", name, name + index);
                return new Tuple<string, SqlParameter>(cd, new SqlParameter("@" + name + index, value));
            }

            if (MethodName == "Equals")
            {
                var name = (MethodCall.Object as MemberExpression).Member.Name;
                var value = constantExp == null
                ? GetValueOfMemberExpression((MemberExpression)MethodCall.Arguments[0])
                : constantExp.Value;
                var cd = string.Format("[{0}] = @{1}", name, name + index);
                return new Tuple<string, SqlParameter>(cd, new SqlParameter("@" + name + index, value));
            }

            if (MethodName == "EndsWith")
            {
                var name = (MethodCall.Object as MemberExpression).Member.Name;
                var value = constantExp == null
                ? GetValueOfMemberExpression((MemberExpression)MethodCall.Arguments[0])
                : constantExp.Value;

                var cd = string.Format("[{0}] like '%'+@{1}", name, name + index);
                return new Tuple<string, SqlParameter>(cd, new SqlParameter("@" + name + index, value));
            }
            return new Tuple<string, SqlParameter>(string.Empty, null);
        }

        private static object GetValueOfMemberExpression(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }

        private static string GetMemberName(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (expression as MemberExpression).Member.Name;
                case ExpressionType.Convert:
                    return GetMemberName(((UnaryExpression)expression).Operand);
                default:
                    return string.Empty;
            }
        }

        private static object GetMemberValue(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    return (expression as ConstantExpression).Value;
                case ExpressionType.MemberAccess:
                    return GetValueOfMemberExpression((MemberExpression)expression);
                case ExpressionType.Convert:
                    return GetMemberValue((expression as UnaryExpression).Operand);
                default:
                    return null;
            }
        }
    }
}
