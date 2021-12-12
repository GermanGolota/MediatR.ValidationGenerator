namespace MediatR.ValidationGenerator
{
    public static class StaticSourceCodes
    {
        public static readonly string Validator = @$"using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace {GlobalNames.InternalNamespace}
{{
    public interface IValidator<in T>
    {{
        {GlobalNames.ValidationResult} Validate(T value);
    }}

    public class {GlobalNames.ValidationResultLocal}
    {{
        public {GlobalNames.ValidationResultLocal}(bool isValid, List<{GlobalNames.ValidationFailure}> errors)
        {{
            IsValid = isValid;
            Errors = errors;
        }}
        //
        // Summary:
        //     Whether validation succeeded
        public virtual bool IsValid {{ get; }}
        //
        // Summary:
        //     A collection of errors
        public List<{GlobalNames.ValidationFailure}> Errors {{ get; }}
    }}

    public class {GlobalNames.ValidationFailureLocal}
    {{
        public {GlobalNames.ValidationFailureLocal}(string propertyName, string errorMessage)
        {{
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }}
        //
        // Summary:
        //     The name of the property.
        public string PropertyName {{ get; set; }}
        //
        // Summary:
        //     The error message
        public string ErrorMessage {{ get; set; }}
    }}

    /// <summary>
    /// An exception that represents failed validation
    /// </summary>
    public class ValidationException : Exception
    {{
        /// <summary>
        /// Validation errors
        /// </summary>
        public IEnumerable<{GlobalNames.ValidationFailure}> Errors {{ get; private set; }}

        /// <summary>
        /// Creates a new ValidationException
        /// </summary>
        /// <param name=""errors""></param>
        public ValidationException(IEnumerable<{GlobalNames.ValidationFailure}> errors) : base(BuildErrorMessage(errors))
        {{
            Errors = errors;
        }}

        private static string BuildErrorMessage(IEnumerable<{GlobalNames.ValidationFailure}> errors)
        {{
            var arr = errors.Select(x => $""{{Environment.NewLine}} -- {{x.PropertyName}}: {{x.ErrorMessage}}"");
            return ""Validation failed: "" + string.Join(string.Empty, arr);
        }}

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {{
            if (info == null) throw new ArgumentNullException(""info"");

            info.AddValue(""errors"", Errors);
            base.GetObjectData(info, context);
        }}
    }}
}}
";

        public static readonly string Behavior = $@"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace {GlobalNames.InternalNamespace}
{{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {{
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {{
            _validators = validators;
        }}

        public Task<TResponse> Handle(TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        )
        {{
            List<{GlobalNames.ValidationFailure}> failures = new List<{GlobalNames.ValidationFailure}>();
            foreach (var validator in _validators)
            {{
                var result = validator.Validate(request);
                if (result.Errors is null == false)
                {{
                    failures.AddRange(result.Errors);
                }}
            }}

            if (failures.Count > 0)
            {{
                throw new ValidationException(failures);
            }}

            return next();
        }}
    }}
}}

";

        public static readonly string DIExtensions = $@"
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using {GlobalNames.InternalNamespace};

namespace MediatR.ValidationGenerator
{{
    public static class DiExtensions
    {{
        public static IServiceCollection AddGeneratedValidators(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies,
            ServiceLifetime lifetime = ServiceLifetime.Transient
            )
        {{
            services.Add(new ServiceDescriptor(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>), lifetime));

            foreach (var assembly in assemblies)
            {{
                services.AddValidatorsFromAssembly(assembly, lifetime);
            }}

            return services;
        }}

        /// <summary>
        /// Adds all validators in specified assembly
        /// </summary>
        /// <param name=""services"">The collection of services</param>
        /// <param name=""assembly"">The assembly to scan</param>
        /// <param name=""lifetime"">The lifetime of the validators. The default is scoped (per-request in web application)</param>
        /// <param name=""filter"">Optional filter that allows certain types to be skipped from registration.</param>
        /// <param name=""includeInternalTypes"">Include internal validators. The default is false.</param>
        /// <returns></returns>
        public static IServiceCollection AddValidatorsFromAssembly(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Scoped, Func<AssemblyScanner.AssemblyScanResult, bool> filter = null, bool includeInternalTypes = false)
        {{
            AssemblyScanner
                .FindValidatorsInAssembly(assembly, includeInternalTypes)
                .ForEach(scanResult => services.AddScanResult(scanResult, lifetime, filter));

            return services;
        }}

        /// <summary>
        /// Helper method to register a validator from an AssemblyScanner result
        /// </summary>
        /// <param name=""services"">The collection of services</param>
        /// <param name=""scanResult"">The scan result</param>
        /// <param name=""lifetime"">The lifetime of the validators. The default is scoped (per-request in web applications)</param>
        /// <param name=""filter"">Optional filter that allows certain types to be skipped from registration.</param>
        /// <returns></returns>
        private static IServiceCollection AddScanResult(this IServiceCollection services, AssemblyScanner.AssemblyScanResult scanResult, ServiceLifetime lifetime, Func<AssemblyScanner.AssemblyScanResult, bool> filter)
        {{
            bool shouldRegister = filter?.Invoke(scanResult) ?? true;
            if (shouldRegister)
            {{
                //Register as interface
                services.Add(
                    new ServiceDescriptor(
                        serviceType: scanResult.InterfaceType,
                        implementationType: scanResult.ValidatorType,
                        lifetime: lifetime));

                //Register as self
                services.Add(
                    new ServiceDescriptor(
                        serviceType: scanResult.ValidatorType,
                        implementationType: scanResult.ValidatorType,
                        lifetime: lifetime));
            }}

            return services;
        }}

        /// <summary>
        /// Class that can be used to find all the validators from a collection of types.
        /// </summary>
        public class AssemblyScanner : IEnumerable<AssemblyScanner.AssemblyScanResult>
        {{
            readonly IEnumerable<Type> _types;

            /// <summary>
            /// Creates a scanner that works on a sequence of types.
            /// </summary>
            public AssemblyScanner(IEnumerable<Type> types)
            {{
                _types = types;
            }}

            /// <summary>
            /// Finds all the validators in the specified assembly.
            /// </summary>
            public static AssemblyScanner FindValidatorsInAssembly(Assembly assembly, bool includeInternalTypes = false)
            {{
                return new AssemblyScanner(includeInternalTypes ? assembly.GetTypes() : assembly.GetExportedTypes());
            }}

            private IEnumerable<AssemblyScanResult> Execute()
            {{
                var openGenericType = typeof(IValidator<>);

                var query = _types.Where(type => (type.IsAbstract || type.IsGenericTypeDefinition) == false)
                    .Select(type =>
                    {{
                        var interfaces = type.GetInterfaces();
                        var genericInterfaces = interfaces.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericType);
                        var matchingInterface = genericInterfaces.FirstOrDefault();
                        return new {{ type, matchingInterface }};
                    }})
                    .Where(pair => pair.matchingInterface is null == false)
                    .Select(x => new AssemblyScanResult(x.matchingInterface, x.type));

                return query;
            }}

            /// <summary>
            /// Performs the specified action to all of the assembly scan results.
            /// </summary>
            public void ForEach(Action<AssemblyScanResult> action)
            {{
                foreach (var result in this)
                {{
                    action(result);
                }}
            }}

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref=""T:System.Collections.Generic.IEnumerator`1""/> that can be used to iterate through the collection.
            /// </returns>
            /// <filterpriority>1</filterpriority>
            public IEnumerator<AssemblyScanResult> GetEnumerator()
            {{
                return Execute().GetEnumerator();
            }}

            IEnumerator IEnumerable.GetEnumerator()
            {{
                return GetEnumerator();
            }}

            /// <summary>
            /// Result of performing a scan.
            /// </summary>
            public class AssemblyScanResult
            {{
                /// <summary>
                /// Creates an instance of an AssemblyScanResult.
                /// </summary>
                public AssemblyScanResult(Type interfaceType, Type validatorType)
                {{
                    InterfaceType = interfaceType;
                    ValidatorType = validatorType;
                }}

                /// <summary>
                /// Validator interface type, eg IValidator&lt;Foo&gt;
                /// </summary>
                public Type InterfaceType {{ get; private set; }}

                /// <summary>
                /// Concrete type that implements the InterfaceType, eg FooValidator.
                /// </summary>
                public Type ValidatorType {{ get; private set; }}
            }}
        }}
    }}
}}
";
    }
}
