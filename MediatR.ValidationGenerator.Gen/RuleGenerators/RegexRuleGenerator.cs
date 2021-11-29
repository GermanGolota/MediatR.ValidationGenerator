using MediatR.ValidationGenerator.Gen.Extensions;
using MediatR.ValidationGenerator.Gen.Models;
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

        public ValueOrNull<string> GenerateRuleFor(AttributeSyntax attribute)
        {
            var arguments = attribute.With(x => x.ArgumentList).With(x => x.Arguments);
            ValueOrNull<string> result;
            if (arguments.IsNotNull())
            {
                string regex = GetRegex(arguments);
                if (regex.IsNotEmpty())
                {
                    result = @$".Must(value => Regex.IsMatch(value.ToString(), ""{regex}"", RegexOptions.None, TimeSpan.FromSeconds(3)))";
                }
                else
                {
                    result = ValueOrNull<string>.CreateNull("No proper regex!");
                }
            }
            else
            {
                result = ValueOrNull<string>.CreateNull("No arguments!");
            }
            return result;
        }

        private static string GetRegex(Microsoft.CodeAnalysis.SeparatedSyntaxList<AttributeArgumentSyntax> arguments)
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