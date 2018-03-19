namespace JSConverter
{
    internal class IndexJsExpression : JsExpression
    {
        public JsExpression Object { get; set; }
        public JsExpression Index { get; set; }

        public IndexJsExpression(JsExpression o, JsExpression index)
        {
            Object = o;
            Index = index;
        }

        public override string ToString()
        {
            return $"{Object}[{Index}]";
        }

        public override void ReplaceConstant(string what, string withWhat)
        {
            Object.ReplaceConstant(what, withWhat);
            Index.ReplaceConstant(what, withWhat);
        }
    }
}
