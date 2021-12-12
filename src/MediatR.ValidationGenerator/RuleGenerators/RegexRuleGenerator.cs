using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MediatR.ValidationGenerator.RuleGenerators
{
    public class RegexRuleGenerator : IRuleGenerator
    {
        private static readonly string _regexGlobal =
            "Regex".GetFromGlobal("System.Text.RegularExpressions");

        private static readonly string _regexOptionsGlobal =
            "RegexOptions".GetFromGlobal($"System.Text.RegularExpressions");

        private static readonly string _timeSpanGlobal = nameof(System.TimeSpan).GetFromGlobal(nameof(System));

        private static readonly string _regexAttributeName = nameof(RegularExpressionAttribute);
        public bool IsMatchingAttribute(AttributeData attribute)
        {
            string attributeName = attribute.AttributeClass?.Name ?? "";
            return AttributeHelper.IsTheSameAttribute(attributeName, _regexAttributeName);
        }

        public SuccessOrFailure GenerateRuleFor(IPropertySymbol prop, AttributeData attribute, MethodBodyBuilder body)
        {
            SuccessOrFailure result;
            string? regex = GetRegex(attribute);
            if (regex is not null)
            {
                string param = RequestValidatorCreator.VALIDATOR_PARAMETER_NAME;
                string fullProp = $"{ param }.{ prop.Name}";
                body.AppendNotEnding($"if({_regexGlobal}.IsMatch({fullProp}, \"{regex}\", {_regexOptionsGlobal}.None, {_timeSpanGlobal}.FromSeconds(3)) == false)");
                body.AppendError($"nameof({fullProp})", "\"Does not fulfill regex\"", true);
                result = true;
            }
            else
            {
                result = SuccessOrFailure.CreateFailure("No proper regex!");
            }
            return result;
        }

        private static string? GetRegex(AttributeData attribute)
        {
            string? regex = null;

            var ctorArgs = attribute.ConstructorArguments;
            if (ctorArgs.Count() != 0)
            {
                regex = ctorArgs.First().Value?.ToString();
            }
            else
            {
                foreach (var arg in attribute.NamedArguments)
                {
                    if (arg.Key == nameof(RegularExpressionAttribute.Pattern))
                    {
                        regex = arg.Value.Value?.ToString();
                        break;
                    }
                }
            }
            return regex;
        }
    }
}