using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen
{
    public static class AttributeService
    {
        private static List<string> _supportedAttributes = new List<string>
        {
            "Required"
        };

        public static bool AttributeIsSupported(AttributeSyntax attribute)
        {
            return _supportedAttributes.Contains(attribute.Name.ToString());
        }

        public static ValueOrNull<string> CreateRuleForAttribute(AttributeSyntax attribute)
        {
            string attributeName = attribute.Name.ToString();

            ValueOrNull<string> result;
            switch (attributeName)
            {
                case "Required":
                    result = ".NotEmpty()";
                    break;
                default:
                    result = ValueOrNull<string>.CreateNull("Unsupported attribute");
                    break;
            }
            return result;
        }
    }
}
