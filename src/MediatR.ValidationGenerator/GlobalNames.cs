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
        public static readonly string Validator = ValidatorLocal
            .GetFromGlobal(InternalNamespace);

        public static readonly string ValidationResultLocal = "ValidationResult";

        public static readonly string ValidationResult = ValidationResultLocal
            .GetFromGlobal(InternalNamespace);

        public static readonly string ValidationExceptionLocal = "ValidationException";
        public static readonly string ValidationException = ValidationExceptionLocal
            .GetFromGlobal(InternalNamespace);

        public static readonly string DIRegistrationsLocal = "DIRegistrations";

        public static readonly string DIRegistrationsDict = "Registrations";

        public static readonly string DIRegistrations = DIRegistrationsLocal
            .GetFromGlobal(InternalNamespace);

        public static readonly string ValidationBehaviorLocal = "ValidationBehavior";
        public static readonly string ValidationBehavior = ValidationBehaviorLocal
            .GetFromGlobal(InternalNamespace);

        public static readonly string String = "String"
           .GetFromGlobal("System");

        public static readonly string Exception = "Exception"
           .GetFromGlobal("System");

        public static readonly string Environment = "Environment"
           .GetFromGlobal("System");

        public static readonly string ArgumentNullException = "ArgumentNullException"
           .GetFromGlobal("System");

        public static readonly string Enumerable = "IEnumerable"
          .GetFromGlobal("System.Collections.Generic");

        public static readonly string List = "List"
          .GetFromGlobal("System.Collections.Generic");

        public static readonly string Dictionary = "Dictionary"
          .GetFromGlobal("System.Collections.Generic");

        public static readonly string Task = "Task"
          .GetFromGlobal("System.Threading.Tasks");

        public static readonly string SerializationInfo = "SerializationInfo"
          .GetFromGlobal("System.Runtime.Serialization");

        public static readonly string StreamingContext = "StreamingContext"
         .GetFromGlobal("System.Runtime.Serialization");

        public static readonly string CancellationToken = "CancellationToken"
            .GetFromGlobal("System.Threading");

        public static readonly string Type = nameof(System.Type)
          .GetFromGlobal(nameof(System));

        public static readonly string Func = "Func"
            .GetFromGlobal("System");

        public static readonly string Lazy = "Lazy"
            .GetFromGlobal("System");

        public static readonly string ServiceProvider = nameof(System.IServiceProvider)
            .GetFromGlobal("System");

        public static readonly string MediNamespace = "Microsoft.Extensions.DependencyInjection";

        public static readonly string ServiceProviderServiceExtensions = "ServiceProviderServiceExtensions"
            .GetFromGlobal(MediNamespace);

        public static readonly string ServiceCollection = "IServiceCollection"
            .GetFromGlobal(MediNamespace);

        public static readonly string ValidatorsNamespace = "Validators.Generated";

        public static readonly string ServiceDescriptor = "ServiceDescriptor"
            .GetFromGlobal(MediNamespace);

        public static readonly string ServiceLifetime = "ServiceLifetime"
            .GetFromGlobal(MediNamespace);

        public static readonly string Attribute = "Attribute"
            .GetFromGlobal("System");

        public static readonly string PipelineBehavior = "IPipelineBehavior"
            .GetFromGlobal("MediatR");

        public static readonly string RequestHandlerDelegate = "RequestHandlerDelegate"
            .GetFromGlobal("MediatR");

        public static readonly string CustomValidatorAttribute = "CustomValidatorAttribute";

    }
}
