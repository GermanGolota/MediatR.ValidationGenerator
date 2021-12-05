using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MediatR.ValidationGenerator.RuleGenerators
{
    public interface IRuleGenerator
    {
        bool IsMatchingAttribute(AttributeData attribute);
        SuccessOrFailure GenerateRuleFor(
            IPropertySymbol prop,
            AttributeData attribute,
            MethodBodyBuilder body);
    }
}
