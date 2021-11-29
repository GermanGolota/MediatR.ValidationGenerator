using MediatR.ValidationGenerator.Gen.Builders;
using MediatR.ValidationGenerator.Gen.Models;
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

        public static List<SuccessOrFailure> AppendRulesForAttribute(
            MethodBodyBuilder builder,
            PropertyDeclarationSyntax prop,
            IEnumerable<AttributeSyntax> attributes
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