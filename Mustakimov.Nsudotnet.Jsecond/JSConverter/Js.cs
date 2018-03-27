using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace JSConverter
{
    public static class Js
    {
        public static string Convert(Expression expr, ICollection<ParameterExpression> pars, string functionName)
        {
            var body = new JsExpressionVisitor().Convert(expr);
            var parameters = string.Join(", ", pars.Select(p => p.Name));

            string fnBody = body.IsExpression ? $"return {body};" : body.ToString();

            return $"function {functionName}({parameters}) {{ {fnBody} }}";
        }

        public static string Convert(Expression<Func<object>> expr, string functionName = "")
        {
            return Convert(expr.Body, expr.Parameters, functionName);
        }
    }
    public static class Js<T1>
    {
        public static string Convert(Expression<Func<T1, object>> expr, string functionName = "")
        {
            return Js.Convert(expr.Body, expr.Parameters, functionName);
        }
    }

    public static class Js<T1, T2>
    {
        public static string Convert(Expression<Func<T1, T2, object>> expr, string functionName = "")
        {
            return Js.Convert(expr.Body, expr.Parameters, functionName);
        }
    }

    public static class Js<T1, T2, T3>
    {
        public static string Convert(Expression<Func<T1, T2, T3, object>> expr, string functionName = "")
        {
            return Js.Convert(expr.Body, expr.Parameters, functionName);
        }
    }

    public static class Js<T1, T2, T3, T4>
    {
        public static string Convert(Expression<Func<T1, T2, T3, T4, object>> expr, string functionName = "")
        {
            return Js.Convert(expr.Body, expr.Parameters, functionName);
        }
    }

    public static class Js<T1, T2, T3, T4, T5>
    {
        public static string Convert(Expression<Func<T1, T2, T3, T4, T5, object>> expr, string functionName = "")
        {
            return Js.Convert(expr.Body, expr.Parameters, functionName);
        }
    }
}
