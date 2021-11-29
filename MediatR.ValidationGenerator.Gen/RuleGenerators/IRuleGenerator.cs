using MediatR.ValidationGenerator.Gen.Builders;
using MediatR.ValidationGenerator.Gen.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MediatR.ValidationGenerator.Gen.RuleGenerators
{
    public interface IRuleGenerator
    {
        bool IsMatchingAttribute(AttributeSyntax attribute);
        SuccessOrFailure GenerateRuleFor(
            PropertyDeclarationSyntax prop, 
            AttributeSyntax attribute, 
            MethodBodyBuilder body);
    }
}
