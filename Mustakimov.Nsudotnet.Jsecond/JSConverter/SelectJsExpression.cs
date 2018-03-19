namespace JSConverter
{
    internal class SelectJsExpression : JsExpression
    {
        public JsExpression From { get; set; }

        public JsExpression Selector { get; set; }

        public string SelectorParameter { get; set; }

        public override bool IsExpression => false;

        public SelectJsExpression(JsExpression @from, JsExpression selector, string selectorParameter)
        {
            From = @from;
            Selector = selector;
            SelectorParameter = selectorParameter;
        }

        public override string ToString()
        {
            if (From is WhereJsExpression whereExpression)
            {
                string from = whereExpression.GetCombinedFrom();
                string selector = Selector.ToString();

                selector = selector.Replace(SelectorParameter, from + "[i]");

                string condition = whereExpression.GetCombinedCondition(from + "[i]");
                return $"var r = []; for(var i=0;i<{from}.length;i++) {{ if {condition} r.push({selector}); }} return r;";
            }
            else
            {
                string from = From.ToString();
                string selector = Selector.ToString();

                selector = selector.Replace(SelectorParameter, from + "[i]");

                return $"var r = []; for(var i=0;i<{from}.length;i++) {{ r.push({selector}); }} return r;";
            }
        }

        public override void ReplaceConstant(string what, string withWhat)
        {
            From.ReplaceConstant(what, withWhat);
        }
    }
}
