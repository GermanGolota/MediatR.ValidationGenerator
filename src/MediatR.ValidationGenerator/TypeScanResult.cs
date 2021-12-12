using Microsoft.CodeAnalysis;

namespace MediatR.ValidationGenerator
{
    public record TypeScanResult(ITypeSymbol Type, bool ImplementsIRequest);
}
