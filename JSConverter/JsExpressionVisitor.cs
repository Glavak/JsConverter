using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

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
                    returnStack.Push(new CoalesceJsOperator(returnStack.Pop(), returnStack.Pop()));
                    break;
                default:
                    returnStack.Push(new BinaryJsExpression(returnStack.Pop(), GetOperator(node.NodeType), returnStack.Pop()));
                    break;
            }

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            Visit(node.Expression);

            returnStack.Push(new MemberAccesJsExpression(returnStack.Pop(), node.Member.Name, false));

            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            return base.VisitMethodCall(node);
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
            returnStack.Push(new ConstantJsExpression(node.Value.ToString()));

            return node;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            return base.VisitUnary(node);
        }
    }
}
