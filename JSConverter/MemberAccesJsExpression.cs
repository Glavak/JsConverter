namespace JSConverter
{
    internal class MemberAccesJsExpression : JsExpression
    {
        public JsExpression Object { get; set; }
        public string Member { get; set; }
        public bool IsMethod { get; set; }

        public MemberAccesJsExpression(JsExpression o, string member, bool isMethod)
        {
            Object = o;
            Member = member;
            IsMethod = isMethod;
        }

        public override string ToString()
        {
            return $"{Object}.{Member}{(IsMethod ? "()" : "")}";
        }
    }
}
