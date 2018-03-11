using System;
using System.Linq;
using System.Linq.Expressions;

namespace JSConverter
{
    public static class Js<T1>
    {
        public static string Convert(Expression<Func<T1, object>> expr, string functionName = "")
        {
            string body = new JsExpressionVisitor().Convert(expr.Body).ToString();
            string parameters = string.Join(", ", expr.Parameters.Select(p => p.Name));
            return $"function {functionName}({parameters}) {{ return {body}; }}";
        }
    }

    public static class Js<T1, T2>
    {
        public static string Convert(Expression<Func<T1, T2, object>> expr, string functionName = "")
        {
            string body = new JsExpressionVisitor().Convert(expr.Body).ToString();
            string parameters = string.Join(", ", expr.Parameters.Select(p => p.Name));
            return $"function {functionName}({parameters}) {{ return {body}; }}";
        }
    }
}
