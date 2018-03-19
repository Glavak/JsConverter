using System;
using System.Linq;
using System.Linq.Expressions;

namespace JSConverter
{
    public static class Js
    {
        public static string Convert(Expression<Func<object>> expr, string functionName = "")
        {
            var body = new JsExpressionVisitor().Convert(expr.Body);
            var parameters = string.Join(", ", expr.Parameters.Select(p => p.Name));

            string fnBody = body.IsExpression ? $"return {body};" : body.ToString();

            return $"function {functionName}({parameters}) {{ {fnBody} }}";
        }
    }
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

    public static class Js<T1, T2, T3>
    {
        public static string Convert(Expression<Func<T1, T2, T3, object>> expr, string functionName = "")
        {
            var body = new JsExpressionVisitor().Convert(expr.Body);
            var parameters = string.Join(", ", expr.Parameters.Select(p => p.Name));

            string fnBody = body.IsExpression ? $"return {body};" : body.ToString();

            return $"function {functionName}({parameters}) {{ {fnBody} }}";
        }
    }

    public static class Js<T1, T2, T3, T4>
    {
        public static string Convert(Expression<Func<T1, T2, T3, T4, object>> expr, string functionName = "")
        {
            var body = new JsExpressionVisitor().Convert(expr.Body);
            var parameters = string.Join(", ", expr.Parameters.Select(p => p.Name));

            string fnBody = body.IsExpression ? $"return {body};" : body.ToString();

            return $"function {functionName}({parameters}) {{ {fnBody} }}";
        }
    }

    public static class Js<T1, T2, T3, T4, T5>
    {
        public static string Convert(Expression<Func<T1, T2, T3, T4, T5, object>> expr, string functionName = "")
        {
            var body = new JsExpressionVisitor().Convert(expr.Body);
            var parameters = string.Join(", ", expr.Parameters.Select(p => p.Name));

            string fnBody = body.IsExpression ? $"return {body};" : body.ToString();

            return $"function {functionName}({parameters}) {{ {fnBody} }}";
        }
    }
}
