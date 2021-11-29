using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;

namespace MediatR.ValidationGenerator.RuleGenerators
{
    public class RegexRuleGenerator : IRuleGenerator
    {
        private readonly string _requiredAttributeName = AttributeHelper.GetProperName(nameof(RegularExpressionAttribute));
        public bool IsMatchingAttribute(AttributeSyntax attribute)
        {
            string attributeName = attribute.Name.ToString();
            return attributeName == _requiredAttributeName;
        }

        public SuccessOrFailure GenerateRuleFor(PropertyDeclarationSyntax prop, AttributeSyntax attribute, MethodBodyBuilder body)
        {
            var arguments = attribute.With(x => x.ArgumentList).With(x => x.Arguments);
            SuccessOrFailure result;
            if (arguments.IsNotNull())
            {
                string regex = GetRegex(arguments);
                if (regex.IsNotEmpty())
                {
                    string errors = RequestValidatorCreator.VALIDATOR_ERRORS_LIST_NAME;
                    string param = RequestValidatorCreator.VALIDATOR_PARAMETER_NAME;
                    string validityFlag = RequestValidatorCreator.VALIDATOR_VALIDITY_NAME;
                    string fullProp = $"{ param }.{ prop.Identifier}";
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
            }
            else
            {
                result = SuccessOrFailure.CreateFailure("No arguments!");
            }
            return result;
        }


        private static string GetRegex(SeparatedSyntaxList<AttributeArgumentSyntax> arguments)
        {
            string regex = null;

            var firstArg = arguments.FirstOrDefault();
            if (firstArg.Expression is LiteralExpressionSyntax literalArg)
            {
                regex = literalArg.Token.Value?.ToString();
            }

            return regex;
        }

    }
}