using Microsoft.CodeAnalysis;

namespace MediatR.ValidationGenerator;

public static class DiagnosticDescriptors
{
    private static DiagnosticDescriptor _failedToCreateValidatorDescriptor = new DiagnosticDescriptor("VG0001",
      "Failed to create validator",
      "Failed to create validator for '{0}' request with message '{1}'",
      "Errors",
      DiagnosticSeverity.Error,
      true);

    public static DiagnosticDescriptor FailedToCreateValidatorDescriptor => _failedToCreateValidatorDescriptor;
}
