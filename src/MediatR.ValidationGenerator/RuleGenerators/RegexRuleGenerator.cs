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
                string errors = RequestValidatorCreator.VALIDATOR_ERRORS_LIST_NAME;
                string param = RequestValidatorCreator.VALIDATOR_PARAMETER_NAME;
                string validityFlag = RequestValidatorCreator.VALIDATOR_VALIDITY_NAME;
                string fullProp = $"{ param }.{ prop.Name}";
                body.AppendLine($"if({_regexGlobal}.IsMatch({fullProp}, \"{regex}\", {_regexOptionsGlobal}.None, {_timeSpanGlobal}.FromSeconds(3)) == false)", endLine: false);
                body.AppendLine("{", endLine: false);
                body.AppendLine($"{errors}.Add(new {GlobalNames.ValidationFailure}(nameof({fullProp}), \"Does not fulfill regex\"))", 1);
                body.AppendLine($"{validityFlag} = false", 1);
                body.AppendLine("}", endLine: false);
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