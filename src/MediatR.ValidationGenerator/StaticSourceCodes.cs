using static MediatR.ValidationGenerator.GlobalNames;

namespace MediatR.ValidationGenerator
{
    public static class StaticSourceCodes
    {
        public static readonly string ValidatorDefinition = @$"using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace {InternalNamespace}
{{
    public interface {ValidatorLocal}<in T>
    {{
        {ValidationResult} Validate(T value);
    }}

    public class {ValidationResultLocal}
    {{
        public {ValidationResultLocal}(bool isValid, List<{ValidationFailure}> errors)
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
        public List<{ValidationFailure}> Errors {{ get; }}
    }}

    public class {ValidationFailureLocal}
    {{
        public {ValidationFailureLocal}(string propertyName, string errorMessage)
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
        public IEnumerable<{ValidationFailure}> Errors {{ get; private set; }}

        /// <summary>
        /// Creates a new ValidationException
        /// </summary>
        /// <param name=""errors""></param>
        public ValidationException(IEnumerable<{ValidationFailure}> errors) : base(BuildErrorMessage(errors))
        {{
            Errors = errors;
        }}

        private static string BuildErrorMessage(IEnumerable<{ValidationFailure}> errors)
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

namespace {InternalNamespace}
{{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {{
        private readonly IEnumerable<{Validator}<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<{Validator}<TRequest>> validators)
        {{
            _validators = validators;
        }}

        public Task<TResponse> Handle(TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next
        )
        {{
            List<{ValidationFailure}> failures = new List<{ValidationFailure}>();
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
using {InternalNamespace};

namespace {PublicNamespace}
{{
    public static class DiExtensions
    {{
        public static IServiceCollection AddGeneratedValidators(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Transient
            )
        {{
            services.Add(new ServiceDescriptor(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>), lifetime));
            var registrationsContainer = new {DIRegistrations}();
            foreach (var registration in registrationsContainer.{DIRegistrationsDict})
            {{
                var interfaceType = registration.Key;       
                var implementationType = registration.Value;
                var descriptor = new {ServiceDescriptor}(interfaceType, implementationType, lifetime);
                services.Add(descriptor);
            }}
            
            return services;
        }}
    }}

    public static class ServiceProviderExtensions
    {{
        public static {ServiceProvider} ApplyToGeneratedValidators(this {ServiceProvider} services)
        {{
            {DIProvider}.ResolveFunction = 
                new {Lazy}<{Func}<{Type}, object>>(
                    () => 
                        (type) => {ServiceProviderServiceExtensions}.GetRequiredService(services, type)
                );
            return services;
        }}
    }}

    public static class {DIProviderLocal}
    {{
        public static {Lazy}<{Func}<{Type}, object>> {ResolveFunction};
    }}
}}

namespace {InternalNamespace}
{{
    //partial class for registrations that would get populated in generated second part of this class
    internal partial class {DIRegistrationsLocal}
    {{
        //interface to implementation
        public {Dictionary}<{Type},{Type}> {DIRegistrationsDict} {{ get; }} 
                = new {Dictionary}<{Type},{Type}>();
    }}
}}
";

        public static readonly string Attributes = $@"
namespace {PublicNamespace}
{{
    public class {CustomValidatorAttribute} : {ValidationAttribute}
    {{
        public {CustomValidatorAttribute}({Type} validatorType, string validationMethod)
        {{
            ValidatorType = validatorType;
            ValidationMethod = validationMethod;
        }}
    
        public {Type} ValidatorType {{ get; set; }}
        public string ValidationMethod {{ get; set; }}
    }}                
}}
";
    }
}


