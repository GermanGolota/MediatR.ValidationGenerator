using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using MediatR.ValidationGenerator.Rules;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MediatR.ValidationGenerator;

public static class AttributeService
{
    public static bool AttributeIsSupported(AttributeData attribute)
    {
        var generators = RulesCollector.Collect();
        return generators.Any(x => x.IsMatchingAttribute(attribute));
    }

    public static List<ITypeSymbol> GetRequiredServices(Dictionary<IPropertySymbol, ImmutableArray<AttributeData>> props)
    {
        List<ITypeSymbol> services = new List<ITypeSymbol>();
        var rules = RulesCollector.Collect();

        foreach (var property in props)
        {
            var prop = property.Key;
            var attributes = property.Value;

            var currentServices = IterateRules(attributes, rules,
                (rule, attribute) => rule.GetRequiredServices(attribute))
                .Flatten();

            services.AddRange(currentServices);
        }

        return services;
    }

    public static List<SuccessOrFailure> AppendRulesForAttribute(
        MethodBodyBuilder builder,
        IPropertySymbol prop,
        IEnumerable<AttributeData> attributes,
        ServicesContainer services
        )
    {
        List<SuccessOrFailure> results = new List<SuccessOrFailure>();
        var rules = RulesCollector.Collect();
        return IterateRules(attributes, rules,
            (rule, attribute) =>
                rule.AppendFor(prop, attribute, builder, services)
        );
    }

    private static List<T> IterateRules<T>(
        IEnumerable<AttributeData> attributes,
        IEnumerable<IRule> rules,
        Func<IRule, AttributeData, T> matching)
    {
        List<T> result = new List<T>();

        foreach (var attribute in attributes)
        {
            foreach (var rule in rules)
            {
                if (rule.IsMatchingAttribute(attribute))
                {
                    result.Add(matching(rule, attribute));
                }
            }
        }

        return result;
    }
}