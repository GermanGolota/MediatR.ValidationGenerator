using MediatR.ValidationGenerator.Gen.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var arguments = attribute.ArgumentList?.Arguments;

            ValueOrNull<string> result;
            switch (attributeName)
            {
                case "Required":
                    result = CreateRequired(arguments);
                    break;
                default:
                    result = ValueOrNull<string>.CreateNull("Unsupported attribute");
                    break;
            }
            return result;
        }

        private static string CreateRequired(SeparatedSyntaxList<AttributeArgumentSyntax>? arguments)
        {
            string result = ".NotEmpty()";
            if (arguments.HasValue)
            {
                var errorMessage = arguments.Value
                                    .Where(x => x.NameEquals.Name.Identifier.ToString() == "ErrorMessage")
                                    .FirstOrDefault();

                if (errorMessage != default)
                {
                    var expression = errorMessage.Expression as LiteralExpressionSyntax;
                    if (expression.IsNotNull())
                    {
                        string actualMessage = expression.Token.Value?.ToString();
                        if (actualMessage.IsNotEmpty())
                        {
                            result += $".WithMessage(\"{actualMessage}\")";
                        }
                    }
                }
            }
            return result;
        }
    }
}
