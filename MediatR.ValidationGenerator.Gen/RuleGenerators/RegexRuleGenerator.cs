using MediatR.ValidationGenerator.Gen.Builders;
using MediatR.ValidationGenerator.Gen.Extensions;
using MediatR.ValidationGenerator.Gen.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.RuleGenerators
{
    public class RegexRuleGenerator : IRuleGenerator
    {
        private readonly string _requiredAttributeName = AttributeHelper.GetProperName(nameof(RegularExpressionAttribute));
        public bool IsMatchingAttribute(AttributeSyntax attribute)
        {
            string attributeName = attribute.Name.ToString();
            return attributeName == _requiredAttributeName;
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

        public ValueOrNull<List<string>> GenerateRuleFor(PropertyDeclarationSyntax prop, AttributeSyntax attribute)
        {
            var arguments = attribute.With(x => x.ArgumentList).With(x => x.Arguments);
            ValueOrNull<List<string>> result;
            if (arguments.IsNotNull())
            {
                string regex = GetRegex(arguments);
                if (regex.IsNotEmpty())
                {
                    List<string> lines = new List<string>();
                    string errors = RequestValidatorCreator.VALIDATOR_ERRORS_LIST_NAME;
                    string param = RequestValidatorCreator.VALIDATOR_PARAMETER_NAME;
                    string validityFlag = RequestValidatorCreator.VALIDATOR_VALIDITY_NAME;
                    string fullProp = $"{ param }.{ prop.Identifier}";
                    lines.Add($"if(Regex.IsMatch({fullProp}, \"{regex}\", RegexOptions.None, TimeSpan.FromSeconds(3)) == false)");
                    lines.Add("{");
                    lines.Add(BuilderUtils.TAB + $"{errors}.Add(new ValidationFailure(\"nameof({fullProp})\", \"Does not fulfill regex\"))");
                    lines.Add(BuilderUtils.TAB + $"{validityFlag} = false");
                    lines.Add("}");
                    result = lines;
                }
                else
                {
                    result = ValueOrNull<List<string>>.CreateNull("No proper regex!");
                }
            }
            else
            {
                result = ValueOrNull<List<string>>.CreateNull("No arguments!");
            }
            return result;
        }
    }
}