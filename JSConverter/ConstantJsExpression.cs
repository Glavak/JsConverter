namespace JSConverter
{
    internal class ConstantJsExpression : JsExpression
    {
        public string ConstantValue { get; set; }

        public ConstantJsExpression(string constantValue)
        {
            ConstantValue = constantValue;
        }

        public override string ToString()
        {
            return ConstantValue;
        }
    }
}
