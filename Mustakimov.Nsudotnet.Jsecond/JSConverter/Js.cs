using System;
using System.Linq;
using System.Linq.Expressions;

namespace JSConverter
{
    public static class Js<T1>
    {
        public static string Convert(Expression<Func<T1, object>> expr, string functionName = "")
        {
            var body = new JsExpressionVisitor().Convert(expr.Body);
            var parameters = string.Join(", ", expr.Parameters.Select(p => p.Name));

            string fnBody = body.IsExpression ? $"return {body};" : body.ToString();

            return $"function {functionName}({parameters}) {{ {fnBody} }}";
        }
    }

    public static class Js<T1, T2>
    {
        public static string Convert(Expression<Func<T1, T2, object>> expr, string functionName = "")
        {
            var body = new JsExpressionVisitor().Convert(expr.Body);
            var parameters = string.Join(", ", expr.Parameters.Select(p => p.Name));

            string fnBody = body.IsExpression ? $"return {body};" : body.ToString();

            return $"function {functionName}({parameters}) {{ {fnBody} }}";
        }
    }
}
