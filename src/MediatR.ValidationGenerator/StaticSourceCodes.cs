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
    public interface {GlobalNames.ValidatorLocal}<in T>
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
        private readonly IEnumerable<{GlobalNames.Validator}<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<{GlobalNames.Validator}<TRequest>> validators)
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

namespace {GlobalNames.PublicNamespace}
{{
    public static class DiExtensions
    {{
        public static IServiceCollection AddGeneratedValidators(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Transient
            )
        {{
            services.Add(new ServiceDescriptor(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>), lifetime));
            var registrationsContainer = new {GlobalNames.DIRegistrations}();
            foreach (var registration in registrationsContainer.{GlobalNames.DIRegistrationsDict})
            {{
                var interfaceType = registration.Key;       
                var implementationType = registration.Value;
                var descriptor = new {GlobalNames.ServiceDescriptor}(interfaceType, implementationType, lifetime);
                services.Add(descriptor);
            }}
            
            return services;
        }}
    }}

    public static class ServiceProviderExtensions
    {{
        public static {GlobalNames.ServiceProvider} ApplyToGeneratedValidators(this {GlobalNames.ServiceProvider} services)
        {{
            {GlobalNames.DIProvider}.ResolveFunction = 
                new {GlobalNames.Lazy}<{GlobalNames.Func}<{GlobalNames.Type}, object>>(
                    () => 
                        (type) => {GlobalNames.ServiceProviderServiceExtensions}.GetRequiredService(services, type)
                );
            return services;
        }}
    }}

    public static class {GlobalNames.DIProviderLocal}
    {{
        public static {GlobalNames.Lazy}<{GlobalNames.Func}<{GlobalNames.Type}, object>> {GlobalNames.ResolveFunction};
    }}
}}

namespace {GlobalNames.InternalNamespace}
{{
    //partial class for registrations that would get populated in generated second part of this class
    internal partial class {GlobalNames.DIRegistrationsLocal}
    {{
        //interface to implementation
        public {GlobalNames.Dictionary}<{GlobalNames.Type},{GlobalNames.Type}> {GlobalNames.DIRegistrationsDict} {{ get; }} 
                = new {GlobalNames.Dictionary}<{GlobalNames.Type},{GlobalNames.Type}>();
    }}
}}
";

        public static readonly string Attributes = $@"
namespace {GlobalNames.PublicNamespace}
{{
    public class {GlobalNames.CustomValidatorAttribute} : {GlobalNames.ValidationAttribute}
    {{
        public {GlobalNames.CustomValidatorAttribute}({GlobalNames.Type} validatorType, string validationMethod)
        {{
            ValidatorType = validatorType;
            ValidationMethod = validationMethod;
        }}
    
        public {GlobalNames.Type} ValidatorType {{ get; set; }}
        public string ValidationMethod {{ get; set; }}
    }}                
}}
";
    }
}


