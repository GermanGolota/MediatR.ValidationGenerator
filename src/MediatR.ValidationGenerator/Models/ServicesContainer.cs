using MediatR.ValidationGenerator.Extensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediatR.ValidationGenerator.Models;

public class ServicesContainer
{
    public ServicesContainer(IEnumerable<ITypeSymbol> types)
    {
        var names = types.Select(type => TypeNameToServiceName(type.Name))
            .PreventDuplicateNames(
                (name, repeated) =>
                    repeated == 0
                        ? name
                        : $"{name}_{repeated}"
            );

        _services = types
            .Zip(names, (type, name) => new { type, name })
            .ToDictionary(x => x.type, x => x.name);
    }

    private string TypeNameToServiceName(string typeName)
    {
        var sb = new StringBuilder(typeName);
        if (sb[0] == 'I')
        {
            sb.Remove(0, 1);
        }

        if (Char.IsUpper(sb[0]))
        {
            sb[0] = Char.ToLower(sb[0]);
        }

        return sb.ToString();
    }

    private readonly Dictionary<ITypeSymbol, string> _services;

    public string GetServiceNameFor(ITypeSymbol type)
    {
        return _services[type];
    }
}
