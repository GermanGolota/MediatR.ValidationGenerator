using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MediatR.ValidationGenerator
{
    public class RequestValidationModel
    {
        public RequestValidationModel(ITypeSymbol requestClass)
        {
            RequestClass = requestClass;
        }

        public ITypeSymbol RequestClass { get; }
        public Dictionary<IPropertySymbol, ImmutableArray<AttributeData>> PropertyToSupportedAttributes { get; } =
            new Dictionary<IPropertySymbol, ImmutableArray<AttributeData>>(SymbolEqualityComparer.Default);

        public string ValidatorName => $"{RequestClass.MetadataName}Validator";
    }
}
