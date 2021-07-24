using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.RuleGenerators
{
    public interface IRuleGenerator
    {
        bool IsMatchingAttribute(AttributeSyntax attribute);
        ValueOrNull<string> GenerateRuleFor(AttributeSyntax attribute);
    }
}
