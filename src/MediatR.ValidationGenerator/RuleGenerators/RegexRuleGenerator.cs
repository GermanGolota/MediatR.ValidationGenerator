using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace MediatR.ValidationGenerator.RuleGenerators
{
    public class RegexRuleGenerator : IRuleGenerator
    {
        private readonly string _requiredAttributeName = AttributeHelper.GetProperName(nameof(RegularExpressionAttribute));
        public bool IsMatchingAttribute(AttributeData attribute)
        {
            string attributeName = attribute.AttributeClass?.Name ?? "";
            return attributeName == _requiredAttributeName;
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
                body.AppendLine($"if(Regex.IsMatch({fullProp}, \"{regex}\", RegexOptions.None, TimeSpan.FromSeconds(3)) == false)", endLine: false);
                body.AppendLine("{", endLine: false);
                body.AppendLine($"{errors}.Add(new ValidationFailure(nameof({fullProp}), \"Does not fulfill regex\"))", 1);
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
            foreach (var arg in attribute.NamedArguments)
            {
                if (arg.Key == nameof(RegularExpressionAttribute.Pattern))
                {
                    regex = arg.Value.Value?.ToString();
                    break;
                }
            }
            return regex;
        }

    }
}