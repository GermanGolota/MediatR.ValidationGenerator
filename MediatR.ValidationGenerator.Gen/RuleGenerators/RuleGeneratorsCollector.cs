using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.RuleGenerators
{
    public static class RuleGeneratorsCollector
    {
        private static readonly IEnumerable<IRuleGenerator> _generators;

        //TODO: Perform reflection magic to get generators
        static RuleGeneratorsCollector()
        {
            _generators = new List<IRuleGenerator>()
            {
                new RequiredRuleGenerator(),
                new RegexRuleGenerator()
            };
        }

        public static IEnumerable<IRuleGenerator> Collect()
        {
            return _generators;
        }
    }
}
