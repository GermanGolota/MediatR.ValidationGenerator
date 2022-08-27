using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace MediatR.ValidationGenerator.Rules;

public interface IRule
{
    bool IsMatchingAttribute(AttributeData attribute);
    IEnumerable<ITypeSymbol> GetRequiredServices(AttributeData attribute);
    SuccessOrFailure AppendFor(
        IPropertySymbol prop,
        AttributeData attribute,
        MethodBodyBuilder body,
        ServicesContainer services);
}
