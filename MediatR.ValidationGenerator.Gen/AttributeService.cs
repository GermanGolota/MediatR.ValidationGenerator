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

        public static IEnumerable<string> CreateRulesForAttribute(AttributeSyntax attribute)
        {
            List<string> rules = new List<string>();
            var generators = RuleGeneratorsCollector.Collect();
            foreach (var generator in generators)
            {
                if (generator.IsMatchingAttribute(attribute))
                {
                    var rule = generator.GenerateRuleFor(attribute);
                    if (rule.HasValue)
                    {
                        rules.Add(rule.Value);
                    }
                }
            }
            return rules;
        }
    }
}
