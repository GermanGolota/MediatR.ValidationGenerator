using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace MediatR.ValidationGenerator.Rules;

public abstract class AttributeRule : IRule
{
    public abstract SuccessOrFailure AppendFor(IPropertySymbol prop, AttributeData attribute, MethodBodyBuilder body, ServicesContainer services);
    public abstract IEnumerable<ITypeSymbol> GetRequiredServices(AttributeData attribute);
    public abstract string AttributeName { get; }
    public bool IsMatchingAttribute(AttributeData attribute)
    {
        string attributeName = attribute.AttributeClass?.Name ?? "";
        return AttributeHelper.IsTheSameAttribute(attributeName, AttributeName);
    }
}
