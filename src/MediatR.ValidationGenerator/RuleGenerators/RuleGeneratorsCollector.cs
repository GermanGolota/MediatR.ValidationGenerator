using System.Collections.Generic;

namespace MediatR.ValidationGenerator.RuleGenerators
{
    public static class RuleGeneratorsCollector
    {
        private static readonly IEnumerable<IRuleGenerator> _generators;

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
