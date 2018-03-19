namespace JSConverter
{
    internal class WhereJsExpression : JsExpression
    {
        public JsExpression From { get; set; }

        public JsExpression Condition { get; set; }

        public string ConditionParameter { get; set; }

        public override bool IsExpression => false;

        public WhereJsExpression(JsExpression @from, JsExpression condition, string conditionParameter)
        {
            From = @from;
            Condition = condition;
            ConditionParameter = conditionParameter;
        }

        public override string ToString()
        {
            string from = From.ToString();
            string condition = Condition.ToString();

            condition = condition.Replace(ConditionParameter, from + "[i]");

            return $"var r = []; for(var i=0;i<{from}.length;i++) {{ if {condition} r.push({from}[i]); }} return r;";
        }

        /// <summary>
        /// Gets combined body of this Where query, and all of it's children queries
        /// </summary>
        public string GetCombinedFrom()
        {
            if (From is WhereJsExpression whereExpression)
            {
                return whereExpression.GetCombinedFrom();
            }
            else
            {
                return From.ToString();
            }
        }

        /// <summary>
        /// Gets combined condition of this Where query, and all of it's children queries
        /// </summary>
        public string GetCombinedCondition(string replaceConditionParameter)
        {
            var ourCondition = Condition.ToString().Replace(ConditionParameter, replaceConditionParameter);
            if (From is WhereJsExpression whereExpression)
            {
                var theirCondition = whereExpression.GetCombinedCondition(replaceConditionParameter);
                return $"({ourCondition} && {theirCondition})";
            }
            else
            {
                return ourCondition;
            }
        }

        public override void ReplaceConstant(string what, string withWhat)
        {
            From.ReplaceConstant(what, withWhat);
            Condition.ReplaceConstant(what, withWhat);
        }
    }
}
