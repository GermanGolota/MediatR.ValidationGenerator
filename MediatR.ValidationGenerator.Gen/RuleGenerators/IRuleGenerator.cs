using MediatR.ValidationGenerator.Gen.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.RuleGenerators
{
    public interface IRuleGenerator
    {
        bool IsMatchingAttribute(AttributeSyntax attribute);
        ValueOrNull<List<string>> GenerateRuleFor(PropertyDeclarationSyntax prop, AttributeSyntax attribute);
    }
}
