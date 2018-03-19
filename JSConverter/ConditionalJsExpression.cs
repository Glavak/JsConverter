namespace JSConverter
{
    internal class ConditionalJsExpression : JsExpression
    {
        public JsExpression Condition { get; set; }
        public JsExpression IfTrue { get; set; }
        public JsExpression IfFalse { get; set; }

        public ConditionalJsExpression(JsExpression condition, JsExpression ifTrue, JsExpression ifFalse)
        {
            Condition = condition;
            IfTrue = ifTrue;
            IfFalse = ifFalse;
        }

        public override string ToString()
        {
            return $"({Condition} ? {IfTrue} : {IfFalse})";
        }

        public override void ReplaceConstant(string what, string withWhat)
        {
            Condition.ReplaceConstant(what, withWhat);
            IfTrue.ReplaceConstant(what, withWhat);
            IfFalse.ReplaceConstant(what, withWhat);
        }
    }
}
