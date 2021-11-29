using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MediatR.ValidationGenerator.RuleGenerators
{
    public class RequiredRuleGenerator : IRuleGenerator
    {
        private readonly string _requiredAttributeName = AttributeHelper.GetProperName(nameof(RequiredAttribute));
        public bool IsMatchingAttribute(AttributeSyntax attribute)
        {
            string attributeName = attribute.Name.ToString();
            return attributeName == _requiredAttributeName;
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

        public SuccessOrFailure GenerateRuleFor(PropertyDeclarationSyntax prop, AttributeSyntax attribute, MethodBodyBuilder body)
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

            //TODO: Generate validation based on prop type
            body.AppendLine($"switch((object){fullProp})", endLine: false);
            body.AppendLine("{", endLine: false);
            List<string> cases = new List<string>()
            {
                "null",
                "string s when String.IsNullOrWhiteSpace(s)",
                "ICollection {Count: 0}",
                "Array {Length: 0}",
                "IEnumerable e when !e.GetEnumerator().MoveNext()"
            };

            foreach (var matchCase in cases)
            {
                body.AppendLine($"case {matchCase}:", 1, endLine: false);
            }
            body.AppendLine($"{errors}.Add(new ValidationFailure(nameof({fullProp}), \"{errorMessage}\"))", 2);
            body.AppendLine($"{validityFlag} = false", 2);
            body.AppendLine("break", 2);
            body.AppendLine("}", endLine: false);

            return true;
        }
    }
}
