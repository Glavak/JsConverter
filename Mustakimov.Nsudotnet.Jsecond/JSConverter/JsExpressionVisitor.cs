using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace JSConverter
{
    internal class JsExpressionVisitor : ExpressionVisitor
    {
        private readonly Stack<JsExpression> returnStack = new Stack<JsExpression>();

        public JsExpression Convert(Expression e)
        {
            Visit(e);

            return returnStack.Pop();
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Right);
            Visit(node.Left);

            switch (node.NodeType)
            {
                case ExpressionType.Coalesce:
                    returnStack.Push(new CoalesceJsExpression(returnStack.Pop(), returnStack.Pop()));
                    break;

                case ExpressionType.ArrayIndex:
                    returnStack.Push(new IndexJsExpression(returnStack.Pop(), returnStack.Pop()));
                    break;

                default:
                    returnStack.Push(new BinaryJsExpression(returnStack.Pop(), GetOperator(node.NodeType), returnStack.Pop()));
                    break;
            }

            return node;
        }

        protected override Expression VisitNew(NewExpression node)
        {
            Dictionary<string, JsExpression> fields = new Dictionary<string, JsExpression>();

            for (int i = 0; i < node.Members.Count; i++)
            {
                Visit(node.Arguments[i]);
                fields[node.Members[i].Name] = returnStack.Pop();
            }

            returnStack.Push(new InitializerJsExpression(fields));

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if(!IsParameter(node))
            {
                string s = Expression.Lambda(node).Compile().DynamicInvoke().ToString();
                returnStack.Push(new ConstantJsExpression(s));
            }
            else
            {
                Visit(node.Expression);

                returnStack.Push(new MemberAccesJsExpression(returnStack.Pop(), node.Member.Name, false));
            }

            return node;
        }

        private static bool IsParameter(Expression expr)
        {
            // если это просто параметр - ну то есть {x}, то да, надо переводить
            if (expr.NodeType == ExpressionType.Parameter) return true;

            // если это не обращение к члену вообще - надо вычислять
            if (!(expr is MemberExpression)) return false;

            // достаем корень цепочки обращений
            var root = GetRootMember((MemberExpression) expr);

            return root?.NodeType == ExpressionType.Parameter;
        }

        private static Expression GetRootMember(MemberExpression expr)
        {
            var accessee = expr.Expression as MemberExpression;
            var current = expr.Expression;
            while (accessee != null)
            {
                accessee = accessee.Expression as MemberExpression;
                if (accessee != null) current = accessee.Expression;
            }
            return current;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name == "Select" &&
                node.Method.Module.Assembly == Assembly.GetAssembly(typeof(System.Linq.Enumerable)))
            {
                var selectorLambda = (LambdaExpression) node.Arguments[1];
                Visit(selectorLambda.Body);
                Visit(node.Arguments[0]);

                returnStack.Push(new SelectJsExpression(returnStack.Pop(), returnStack.Pop(), selectorLambda.Parameters[0].ToString()));
            }
            else if (node.Method.Name == "Where" &&
                     node.Method.Module.Assembly == Assembly.GetAssembly(typeof(System.Linq.Enumerable)))
            {
                var conditionLambda = (LambdaExpression)node.Arguments[1];
                Visit(conditionLambda.Body);
                Visit(node.Arguments[0]);

                returnStack.Push(new WhereJsExpression(returnStack.Pop(), returnStack.Pop(), conditionLambda.Parameters[0].ToString()));
            }
            else if (node.Method.IsSpecialName && node.Method.Name.StartsWith("get_"))
            {
                string possibleProperty = node.Method.Name.Substring(4);
                var properties = node.Method.DeclaringType.GetProperties()
                    .Where(p => p.Name == possibleProperty);

                var property = properties.FirstOrDefault();
                if (property != null)
                {
                    Visit(node.Arguments.First());
                    Visit(node.Object);

                    returnStack.Push(new IndexJsExpression(returnStack.Pop(), returnStack.Pop()));
                }
            }
            else if (node.Method.DeclaringType == typeof(string) && node.Method.Name == "ToUpper")
            {
                Visit(node.Object);
                returnStack.Push(new MemberAccesJsExpression(returnStack.Pop(), "toUpperCase", true));
            }
            else if (node.Method.DeclaringType == typeof(string) && node.Method.Name == "ToLower")
            {
                Visit(node.Object);
                returnStack.Push(new MemberAccesJsExpression(returnStack.Pop(), "toLowerCase", true));
            }
            else if (node.Method.DeclaringType == typeof(string) && node.Method.Name == "Trim")
            {
                Visit(node.Object);
                returnStack.Push(new MemberAccesJsExpression(returnStack.Pop(), "trim", true));
            }
            else
            {
                throw new ArgumentException("Method calling is not supported");
            }

            return node;
        }

        private static string GetOperator(ExpressionType nodeType)
        {
            switch (nodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.AddAssign: 
                case ExpressionType.AddAssignChecked:
                    return "+=";
                case ExpressionType.Assign:
                    return "=";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.DivideAssign:
                    return "/=";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                    return "-=";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                    return "*=";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.Equal:
                    return "==";
                case ExpressionType.NotEqual:
                    return "!=";
                case ExpressionType.Modulo:
                    return "%";
                case ExpressionType.LeftShift:
                    return "<<";
                case ExpressionType.RightShift:
                    return ">>";
                case ExpressionType.And:
                    return "&";
                case ExpressionType.Or:
                    return "|";

                default:
                    throw new InvalidEnumArgumentException(nameof(nodeType), (int) nodeType, typeof(ExpressionType));
                        
            }
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            Visit(node.IfFalse);
            Visit(node.IfTrue);
            Visit(node.Test);

            returnStack.Push(new ConditionalJsExpression(returnStack.Pop(), returnStack.Pop(), returnStack.Pop()));

            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            returnStack.Push(new ConstantJsExpression(node.Name));

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            returnStack.Push(new ConstantJsExpression(node.Value?.ToString() ?? "null"));

            return node;
        }
    }
}
