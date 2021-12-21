using MediatR.ValidationGenerator.Extensions;

namespace MediatR.ValidationGenerator
{
    public static class GlobalNames
    {
        public static readonly string PublicNamespace = "MediatR.ValidationGenerator";
        public static readonly string InternalNamespace = "MediatR.ValidationGenerator.Internal";

        public static readonly string ValidationFailureLocal = "ValidationFailure";

        public static readonly string ValidationFailure = ValidationFailureLocal
            .GetFromGlobal(InternalNamespace);

        public static readonly string ValidatorLocal = "IValidator";
        public static readonly string Validator= ValidatorLocal
            .GetFromGlobal(InternalNamespace);

        public static readonly string ValidationResultLocal = "ValidationResult";

        public static readonly string ValidationResult = ValidationResultLocal
            .GetFromGlobal(InternalNamespace);

        public static readonly string DIRegistrationsLocal = "DIRegistrations";

        public static readonly string DIRegistrationsDict = "Registrations";

        public static readonly string DIRegistrations = DIRegistrationsLocal
            .GetFromGlobal(InternalNamespace);

        public static readonly string String = "String"
           .GetFromGlobal("System");

        public static readonly string List = "List"
          .GetFromGlobal("System.Collections.Generic");

        public static readonly string Dictionary = "Dictionary"
          .GetFromGlobal("System.Collections.Generic");

        public static readonly string Type = nameof(System.Type)
          .GetFromGlobal(nameof(System));

        public static readonly string Func = "Func"
            .GetFromGlobal("System");

        public static readonly string Lazy = "Lazy"
            .GetFromGlobal("System");

        public static readonly string ServiceProvider = nameof(System.IServiceProvider)
            .GetFromGlobal("System");

        public static readonly string ServiceProviderServiceExtensions = "ServiceProviderServiceExtensions"
            .GetFromGlobal("Microsoft.Extensions.DependencyInjection");


        public static readonly string ValidatorsNamespace = "Validators.Generated";

        public static readonly string ServiceDescriptor = "ServiceDescriptor"
            .GetFromGlobal("Microsoft.Extensions.DependencyInjection");

        public static readonly string ValidationAttribute = "ValidationAttribute"
            .GetFromGlobal("System.ComponentModel.DataAnnotations");

        public static readonly string CustomValidatorAttribute = "CustomValidatorAttribute";
        public static readonly string DIProviderLocal = "DIProvider";
        public static readonly string DIProvider = DIProviderLocal
            .GetFromGlobal(PublicNamespace);

        public static readonly string ResolveFunction = "ResolveFunction";
        public static readonly string ResolveFunctionFull = $"{DIProvider}.{ResolveFunction}";
    }
}
