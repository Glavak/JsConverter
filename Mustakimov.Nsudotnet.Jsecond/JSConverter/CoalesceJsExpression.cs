namespace JSConverter
{
    internal class CoalesceJsExpression : JsExpression
    {
        public JsExpression Expression { get; set; }
        public JsExpression IfNull { get; set; }

        public CoalesceJsExpression(JsExpression expression, JsExpression ifNull)
        {
            Expression = expression;
            IfNull = ifNull;
        }

        public override string ToString()
        {
            return $"(({Expression} == null || {Expression} == undefined) ? {IfNull} : {Expression})";
        }

        public override void ReplaceConstant(string what, string withWhat)
        {
            Expression.ReplaceConstant(what, withWhat);
            IfNull.ReplaceConstant(what, withWhat);
        }
    }
}
