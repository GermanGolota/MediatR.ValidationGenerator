using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis;
using System;
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

            string fullProp = $"{ param }.{ prop.Name}";
            string? errorMessage = GetCustomErrorMessageOrNull(attribute);
            if (errorMessage is null)
            {
                errorMessage = "\"Empty required value\"";
            }

            string condition = GetCondition(prop.Type, fullProp);
            body.AppendNotEnding($"if({condition})");
            body.AppendError($"nameof({fullProp})", errorMessage, true);
            return true;
        }

        private static string GetCondition(ITypeSymbol type, string fullProp)
        {
            string condition;
            if (type.IsType("System.String"))
            {
                condition = $"{GlobalNames.String}.IsNullOrWhiteSpace({fullProp})";
            }
            else
            {
                string firstCondition;
                if (type.IsValueType)
                {
                    string typeGlobalName = type.GetGlobalName();
                    firstCondition = $"{fullProp}.Equals(default({typeGlobalName}))";
                }
                else
                {
                    firstCondition = $"{fullProp} == null";
                }

                string? secondCondition = null;
                if (type.IsImplementing("ICollection", "System.Collections")
                   || type.IsImplementing("ICollection`1", "System.Collections.Generic"))
                {
                    secondCondition = $"{fullProp}.Count == 0";
                }
                else
                {
                    if (type.IsImplementing("IEnumerable", "System.Collections"))
                    {
                        secondCondition = $"{fullProp}.GetEnumerator().MoveNext() == false";
                    }
                }
                string second = secondCondition is null ? "" : $" && {secondCondition}";
                condition = $"{firstCondition} { second }";
            }

            return condition;
        }
    }
}
