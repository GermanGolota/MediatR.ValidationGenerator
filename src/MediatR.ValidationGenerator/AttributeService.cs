using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;
using MediatR.ValidationGenerator.RuleGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ValidationGenerator
{
    public static class AttributeService
    {
        public static bool AttributeIsSupported(AttributeData attribute)
        {
            var generators = RuleGeneratorsCollector.Collect();
            return generators.Any(x => x.IsMatchingAttribute(attribute));
        }

        public static List<SuccessOrFailure> AppendRulesForAttribute(
            MethodBodyBuilder builder,
            IPropertySymbol prop,
            IEnumerable<AttributeData> attributes
            )
        {
            List<SuccessOrFailure> successOrFailures = new List<SuccessOrFailure>();
            var generators = RuleGeneratorsCollector.Collect();
            foreach (var attribute in attributes)
            {
                foreach (var generator in generators)
                {
                    if (generator.IsMatchingAttribute(attribute))
                    {
                        var result = generator.GenerateRuleFor(prop, attribute, builder);
                        successOrFailures.Add(result);
                    }
                }
            }
            return successOrFailures;
        }
    }
}