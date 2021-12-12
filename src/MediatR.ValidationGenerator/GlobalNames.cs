using MediatR.ValidationGenerator.Extensions;

namespace MediatR.ValidationGenerator
{
    public static class GlobalNames
    {
        public static readonly string InternalNamespace = "MediatR.ValidationGenerator.Internal";

        public static readonly string ValidationFailureLocal = "ValidationFailure";

        public static readonly string ValidationFailure = ValidationFailureLocal
            .GetFromGlobal(InternalNamespace);

        public static readonly string ValidationResultLocal = "ValidationResult";

        public static readonly string ValidationResult = ValidationResultLocal
            .GetFromGlobal(InternalNamespace);

        public static readonly string String = "String"
           .GetFromGlobal("System");

        public static readonly string List = "List"
          .GetFromGlobal("System.Collections.Generic");
    }
}
