using MediatR.ValidationGenerator.Gen.RuleGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ValidationGenerator.Gen
{
    public static class AttributeService
    {
        private static List<IRuleGenerator> _generators;

        static AttributeService()
        {
            _generators = new List<IRuleGenerator>()
            {
                new RequiredRuleGenerator()
            };
        }

        public static bool AttributeIsSupported(AttributeSyntax attribute)
        {
            return _generators.Any(x => x.IsMatchingAttribute(attribute));
        }

        //TODO: Support for multiple rules for one attribute
        public static ValueOrNull<string> CreateRuleForAttribute(AttributeSyntax attribute)
        {
            ValueOrNull<string> result = ValueOrNull<string>.CreateNull("Unsupported attribute");
            foreach (var generator in _generators)
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
