using MediatR.ValidationGenerator.Gen.Extensions;
using MediatR.ValidationGenerator.Gen.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MediatR.ValidationGenerator.Gen.RuleGenerators
{
    public class RequiredRuleGenerator : IRuleGenerator
    {
        private readonly string _requiredAttributeName = AttributeHelper.GetProperName(nameof(RequiredAttribute));
        public bool IsMatchingAttribute(AttributeSyntax attribute)
        {
            string attributeName = attribute.Name.ToString();
            return attributeName == _requiredAttributeName;
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
