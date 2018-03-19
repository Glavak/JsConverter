namespace JSConverter
{
    internal abstract class JsExpression
    {
        public abstract void ReplaceConstant(string what, string withWhat);

        public virtual bool IsExpression => true;
    }
}
