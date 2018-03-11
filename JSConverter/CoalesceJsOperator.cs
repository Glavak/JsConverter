using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSConverter
{
    internal class CoalesceJsOperator : JsExpression
    {
        public JsExpression Expression { get; set; }
        public JsExpression IfNull { get; set; }

        public CoalesceJsOperator(JsExpression expression, JsExpression ifNull)
        {
            Expression = expression;
            IfNull = ifNull;
        }

        public override string ToString()
        {
            return $"(({Expression} == null || {Expression} == undefined) ? {IfNull} : {Expression})";
        }
    }
}
