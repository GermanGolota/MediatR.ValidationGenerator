using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MediatR.ValidationGenerator.RuleGenerators
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
