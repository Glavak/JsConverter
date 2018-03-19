using System.Collections.Generic;
using System.Linq;

namespace JSConverter
{
    internal class InitializerJsExpression : JsExpression
    {
        public Dictionary<string, JsExpression> Fields { get; set; }

        public InitializerJsExpression(Dictionary<string, JsExpression> fields)
        {
            Fields = fields;
        }

        public override string ToString()
        {
            string fields = string.Join(", ", Fields.Select(pair => $"{pair.Key}: {pair.Value}"));
            return $"{{ {fields} }}";
        }

        public override void ReplaceConstant(string what, string withWhat)
        {
            foreach (var field in Fields.Values)
            {
                field.ReplaceConstant(what, withWhat);
            }
        }
    }
}
