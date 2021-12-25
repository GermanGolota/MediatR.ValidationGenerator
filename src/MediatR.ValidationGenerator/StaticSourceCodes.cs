using static MediatR.ValidationGenerator.GlobalNames;

namespace MediatR.ValidationGenerator
{
    public static class StaticSourceCodes
    {
        public static readonly string ValidatorDefinition = @$"
namespace {InternalNamespace}
{{
    public interface {ValidatorLocal}<in T>
    {{
        {ValidationResult} Validate(T value);
    }}

    public class {ValidationResultLocal}
    {{
        public {ValidationResultLocal}(bool isValid, {List}<{ValidationFailure}> errors)
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
        public {List}<{ValidationFailure}> Errors {{ get; }}
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
    public class {ValidationExceptionLocal} : {Exception}
    {{
        /// <summary>
        /// Validation errors
        /// </summary>
        public {Enumerable}<{ValidationFailure}> Errors {{ get; private set; }}

        /// <summary>
        /// Creates a new ValidationException
        /// </summary>
        /// <param name=""errors""></param>
        public {ValidationExceptionLocal}({Enumerable}<{ValidationFailure}> errors) : base(BuildErrorMessage(errors))
        {{
            Errors = errors;
        }}

        private static string BuildErrorMessage({Enumerable}<{ValidationFailure}> errors)
        {{
            {List}<string> msgs = new {List}<string>();
            foreach(var error in errors)
            {{
                msgs.Add($""{{({Environment}.NewLine)}} -- {{error.PropertyName}}: {{error.ErrorMessage}}"");
            }}
            return ""Validation failed: "" + {String}.Join({String}.Empty, msgs);
        }}

        public override void GetObjectData({SerializationInfo} info, {StreamingContext} context)
        {{
            if (info == null) throw new {ArgumentNullException}(""info"");

            info.AddValue(""errors"", Errors);
            base.GetObjectData(info, context);
        }}
    }}
}}
";

        public static readonly string Behavior = $@"
namespace {InternalNamespace}
{{
    public class {ValidationBehaviorLocal}<TRequest, TResponse> : {PipelineBehavior}<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {{
        private readonly {Enumerable}<{Validator}<TRequest>> _validators;

        public {ValidationBehaviorLocal}({Enumerable}<{Validator}<TRequest>> validators)
        {{
            _validators = validators;
        }}

        public {Task}<TResponse> Handle(TRequest request,
            {CancellationToken} cancellationToken,
            {RequestHandlerDelegate}<TResponse> next
        )
        {{
            {List}<{ValidationFailure}> failures = new {List}<{ValidationFailure}>();
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
                throw new {ValidationException}(failures);
            }}

            return next();
        }}
    }}
}}

";

        public static readonly string DIExtensions = $@"
namespace {PublicNamespace}
{{
    public static class DiExtensions
    {{
        public static {ServiceCollection} AddGeneratedValidators(
            this {ServiceCollection} services,
            {ServiceLifetime} lifetime = {ServiceLifetime}.Transient
            )
        {{
            services.Add(new {ServiceDescriptor}(typeof({PipelineBehavior}<,>), typeof({ValidationBehavior}<,>), lifetime));
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
    public class {CustomValidatorAttribute} : {Attribute}
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


