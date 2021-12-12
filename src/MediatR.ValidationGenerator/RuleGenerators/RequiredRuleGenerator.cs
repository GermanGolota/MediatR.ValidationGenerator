using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediatR.ValidationGenerator.RuleGenerators
{
    public class RequiredRuleGenerator : IRuleGenerator
    {
        private readonly string _requiredAttributeName = nameof(RequiredAttribute);
        public bool IsMatchingAttribute(AttributeData attribute)
        {
            string attributeName = attribute.AttributeClass?.Name ?? "";
            return AttributeHelper.IsTheSameAttribute(attributeName, _requiredAttributeName);
        }

        private static string? GetCustomErrorMessageOrNull(AttributeData attribute)
        {
            string? customErrorMessage = null;
            var args = attribute.NamedArguments;
            foreach (var arg in args)
            {
                var argName = arg.Key;
                if (argName == "ErrorMessage")
                {
                    var argVal = arg.Value;
                    customErrorMessage = argVal.Value?.ToString();
                    break;
                }
            }
            return customErrorMessage;
        }

        public SuccessOrFailure GenerateRuleFor(IPropertySymbol prop, AttributeData attribute, MethodBodyBuilder body)
        {
            string param = RequestValidatorCreator.VALIDATOR_PARAMETER_NAME;
            string errors = RequestValidatorCreator.VALIDATOR_ERRORS_LIST_NAME;
            string validityFlag = RequestValidatorCreator.VALIDATOR_VALIDITY_NAME;

            string fullProp = $"{ param }.{ prop.Name}";
            string? errorMessage = GetCustomErrorMessageOrNull(attribute);
            if (errorMessage is null)
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
