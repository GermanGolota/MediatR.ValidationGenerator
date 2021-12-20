using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis;

namespace MediatR.ValidationGenerator.RuleGenerators
{
    public abstract class AttributeRuleGenerator : IRuleGenerator
    {
        public abstract SuccessOrFailure GenerateRuleFor(IPropertySymbol prop, AttributeData attribute, MethodBodyBuilder body);
        public abstract string AttributeName { get; }

        public bool IsMatchingAttribute(AttributeData attribute)
        {
            string attributeName = attribute.AttributeClass?.Name ?? "";
            return AttributeHelper.IsTheSameAttribute(attributeName, AttributeName);
        }
    }
}
