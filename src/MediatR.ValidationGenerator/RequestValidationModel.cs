using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MediatR.ValidationGenerator;

public class RequestValidationModel
{
    public RequestValidationModel(ITypeSymbol requestClass)
    {
        RequestClass = requestClass;
    }

    public ITypeSymbol RequestClass { get; }
    public Dictionary<IPropertySymbol, ImmutableArray<AttributeData>> PropertyToSupportedAttributes { get; }
#pragma warning disable RS1024 // Compare symbols correctly
        = new Dictionary<IPropertySymbol, ImmutableArray<AttributeData>>(SymbolEqualityComparer.Default);
#pragma warning restore RS1024 // Compare symbols correctly

    public string ValidatorName => $"{RequestClass.MetadataName}Validator";
}
