using MediatR.ValidationGenerator.Gen.Builders;
using MediatR.ValidationGenerator.Gen.Extensions;
using MediatR.ValidationGenerator.Gen.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
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

        public ValueOrNull<List<string>> GenerateRuleFor(PropertyDeclarationSyntax prop, AttributeSyntax attribute)
        {
            string param = RequestValidatorCreator.VALIDATOR_PARAMETER_NAME;
            string errors = RequestValidatorCreator.VALIDATOR_ERRORS_LIST_NAME;
            string validityFlag = RequestValidatorCreator.VALIDATOR_VALIDITY_NAME;

            string fullProp = $"{ param }.{ prop.Identifier}";
            string errorMessage = GetCustomErrorMessageOrNull(attribute);
            if (errorMessage.IsEmpty())
            {
                errorMessage = "Empty required valued";
            }

            List<string> lines = new List<string>();
            lines.Add($"switch({fullProp})");
            lines.Add("{");
            List<string> cases = new List<string>()
            {
                "null",
                "string s when String.IsNullOrWhiteSpace(s)",
                "ICollection {Count: 0}",
                "Array {Length: 0}",
                "IEnumerable e when !e.GetEnumerator().MoveNext():"
            };

            foreach (var matchCase in cases)
            {
                lines.Add($"{BuilderUtils.TAB}case {matchCase}:");
            }
            lines.Add($"{BuilderUtils.TAB}{BuilderUtils.TAB} {errors}.Add(new ValidationFailure(\"nameof({fullProp})\", \"{errorMessage}\"))");
            lines.Add($"{BuilderUtils.TAB}{BuilderUtils.TAB} {validityFlag} = false\")");
            lines.Add("}");

            return lines;
        }

        private static string GetCustomErrorMessageOrNull(AttributeSyntax attribute)
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
