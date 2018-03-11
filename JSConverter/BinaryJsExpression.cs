namespace JSConverter
{
    internal class BinaryJsExpression : JsExpression
    {
        public string Operator { get; set; }
        public JsExpression Left { get; set; }
        public JsExpression Right { get; set; }

        public BinaryJsExpression(JsExpression left, string @operator, JsExpression right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public override string ToString()
        {
            return $"({Left} {Operator} {Right})";
        }
    }
}
