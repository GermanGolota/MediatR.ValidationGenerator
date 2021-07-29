using FluentValidation;
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
            return null;
        }
    }
}