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
            string customeErrorMessage = GetCustomeErrorMessageOrNull(attribute);
            if (customeErrorMessage.IsNotEmpty())
            {
                result += $".WithMessage(\"{customeErrorMessage}\")";
            }
            return result;
        }

        private static string GetCustomeErrorMessageOrNull(AttributeSyntax attribute)
        {
            string customeErrorMessage = null;
            var arguments = attribute.ArgumentList?.Arguments;
            if (arguments.HasValue)
            {
                var errorMessages = arguments.Value
                                    .Where(x => x.NameEquals.Name.Identifier.ToString() == "ErrorMessage");

                if (errorMessages.Any())
                {
                    var errorMessage = errorMessages.First();
                    if (errorMessage.Expression is LiteralExpressionSyntax literalSyntax)
                    {
                        customeErrorMessage = literalSyntax.Token.Value?.ToString();
                    }
                }
            }

            return customeErrorMessage;
        }
    }
}
