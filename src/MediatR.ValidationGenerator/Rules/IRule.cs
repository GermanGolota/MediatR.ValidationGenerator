using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis;

namespace MediatR.ValidationGenerator.Rules
{
    public interface IRule
    {
        bool IsMatchingAttribute(AttributeData attribute);
        SuccessOrFailure AppendFor(
            IPropertySymbol prop,
            AttributeData attribute,
            MethodBodyBuilder body);
    }
}
