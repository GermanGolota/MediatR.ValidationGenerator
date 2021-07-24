using MediatR.ValidationGenerator.Gen.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.RuleGenerators
{
    public class RequiredRuleGenerator : IRuleGenerator
    {
        public bool IsMatchingAttribute(AttributeSyntax attribute)
        {
            string attributeName = attribute.Name.ToString();
            return attributeName == "Required";
        }

        public ValueOrNull<string> GenerateRuleFor(AttributeSyntax attribute)
        {
            string result = ".NotEmpty()";
            var arguments = attribute.ArgumentList?.Arguments;

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
