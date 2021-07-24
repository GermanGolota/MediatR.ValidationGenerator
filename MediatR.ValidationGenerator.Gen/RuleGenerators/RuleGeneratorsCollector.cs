using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.RuleGenerators
{
    public static class RuleGeneratorsCollector
    {
        private static readonly IEnumerable<IRuleGenerator> _generators;

        static RuleGeneratorsCollector()
        {
            _generators = new List<IRuleGenerator>()
            {
                new RequiredRuleGenerator()
            };
        }

        public static IEnumerable<IRuleGenerator> Collect()
        {
            return _generators;
        }
    }
}
