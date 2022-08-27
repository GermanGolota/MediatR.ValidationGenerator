using System.Collections.Generic;

namespace MediatR.ValidationGenerator.Rules;

public static class RulesCollector
{
    private static readonly IEnumerable<IRule> _rules = new List<IRule>()
        {
            new RequiredRule(),
            new RegexRule(),
            new CustomValidatorRule()
        };

    public static IEnumerable<IRule> Collect()
    {
        return _rules;
    }
}
