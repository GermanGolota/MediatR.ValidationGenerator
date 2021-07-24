using MediatR.ValidationGenerator.Gen.RuleGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ValidationGenerator.Gen
{
    public static class AttributeService
    {
        public static bool AttributeIsSupported(AttributeSyntax attribute)
        {
            var generators = RuleGeneratorsCollector.Collect();
            return generators.Any(x => x.IsMatchingAttribute(attribute));
        }

        //TODO: Support for multiple rules for one attribute
        public static ValueOrNull<string> CreateRuleForAttribute(AttributeSyntax attribute)
        {
            ValueOrNull<string> result = ValueOrNull<string>.CreateNull("Unsupported attribute");
            var generators = RuleGeneratorsCollector.Collect();
            foreach (var generator in generators)
            {
                if (generator.IsMatchingAttribute(attribute))
                {
                    result = generator.GenerateRuleFor(attribute);
                    break;
                }
            }
            return result;
        }
    }
}
