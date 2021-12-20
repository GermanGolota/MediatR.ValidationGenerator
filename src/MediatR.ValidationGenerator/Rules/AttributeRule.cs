using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis;

namespace MediatR.ValidationGenerator.Rules
{
    public abstract class AttributeRule : IRule
    {
        public abstract SuccessOrFailure AppendFor(IPropertySymbol prop, AttributeData attribute, MethodBodyBuilder body);
        public abstract string AttributeName { get; }
        public bool IsMatchingAttribute(AttributeData attribute)
        {
            string attributeName = attribute.AttributeClass?.Name ?? "";
            return AttributeHelper.IsTheSameAttribute(attributeName, AttributeName);
        }
    }
}
