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

        public static string CreateRuleForAttribute(AttributeSyntax attribute)
        {
            string attributeName = attribute.Name.ToString();

            string result;
            switch (attributeName)
            {
                case "Required":
                    result = ".NotEmpty()";
                    break;
                default:
                    result = "";
                    break;
            }
            return result;
        }
    }
}
