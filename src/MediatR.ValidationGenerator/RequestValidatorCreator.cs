using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;

namespace MediatR.ValidationGenerator
{
    public static class RequestValidatorCreator
    {
        public static readonly string VALIDATE_METHOD_NAME = "Validate";
        public static readonly string VALIDATOR_PARAMETER_NAME = "value";
        public static readonly string VALIDATOR_ERRORS_LIST_NAME = "failures";
        public static readonly string VALIDATOR_VALIDITY_NAME = "isValid";

        public static readonly string VALIDATORS_NAMESPACE = "Validators.Generated";
        public static ValueOrNull<string> CreateValidatorFor(RequestValidationModel model)
        {
            string requestClassName = model.RequestClass.MetadataName;
            string requestNamespace = model.RequestClass.ContainingNamespace.ToDisplayString();

            var classBuilder = new ClassBuilder()
                     .WithClassName($"{model.ValidatorName}")
                     .WithNamespace(VALIDATORS_NAMESPACE)
                     .UsingNamespace("System")
                     .UsingNamespace("System.Collections")
                     .UsingNamespace("System.Collections.Generic")
                     .UsingNamespace("System.Text.RegularExpressions")
                     .UsingNamespace(GlobalNames.InternalNamespace)
                     .UsingNamespace(requestNamespace)
                     .Implementing($"IValidator<{requestClassName}>")
                     .WithMethod(method =>
                     {
                         return method.WithName(VALIDATE_METHOD_NAME)
                                .WithReturnType("ValidationResult")
                                .WithParameter(requestClassName, VALIDATOR_PARAMETER_NAME)
                                .WithBody(body =>
                                {
                                    body
                                       .AppendLine($"bool {VALIDATOR_VALIDITY_NAME} = true")
                                       .AppendLine($"List<ValidationFailure> {VALIDATOR_ERRORS_LIST_NAME} = new List<ValidationFailure>()");

                                    foreach (var entry in model.PropertyToSupportedAttributes)
                                    {
                                        var prop = entry.Key;
                                        var attributes = entry.Value;

                                        body.AppendLine($"#region {prop.Name}Validation");
                                        //TODO: diagnostics
                                        var results = AttributeService.AppendRulesForAttribute(body, prop, attributes);
                                        body.AppendLine($"#endregion");
                                    }

                                    body.AppendLine($"return new ValidationResult({VALIDATOR_VALIDITY_NAME}, {VALIDATOR_ERRORS_LIST_NAME})");

                                    return body;
                                });
                     });

            return classBuilder.Build();
        }
    }
}