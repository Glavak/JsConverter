using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
