using MediatR.ValidationGenerator.Gen.Builders;
using MediatR.ValidationGenerator.Gen.Models;
using MediatR.ValidationGenerator.Gen.RoslynUtils;

namespace MediatR.ValidationGenerator.Gen
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
            string requestClassName = model.RequestClass.Identifier.ToString();
            string requestNamespace = GetRequestNamespace(model);

            var classBuilder = new ClassBuilder()
                     .WithClassName($"{model.ValidatorName}<{requestClassName}>")
                     .WithNamespace(VALIDATORS_NAMESPACE)
                     .UsingNamespace("System")
                     .UsingNamespace(requestNamespace)
                     .Implementing($"AbstractValidator<{requestClassName}>")
                     .WithMethod(method =>
                     {
                         return method.WithName(VALIDATE_METHOD_NAME)
                                .WithReturnType("ValidationResult")
                                .WithParameter("T", VALIDATOR_PARAMETER_NAME)
                                .WithBody(body =>
                                {
                                    body
                                       .AppendLine($"bool {VALIDATOR_VALIDITY_NAME} = true")
                                       .AppendLine($"List<ValidationFailure> {VALIDATOR_ERRORS_LIST_NAME} = new List<ValidationFailure>()");

                                    foreach (var entry in model.PropertyToSupportedAttributes)
                                    {
                                        var prop = entry.Key;
                                        var attributes = entry.Value;

                                        body.AppendLine($"#region {prop.Identifier}Validation");
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

        private static string GetRequestNamespace(RequestValidationModel model)
        {
            var namespaceResult = SyntaxUtils.GetNamespace(model.RequestClass);
            string requestNamespace;
            if (namespaceResult.IsNull)
            {
                requestNamespace = "System.Linq"; //TODO: Output error message
            }
            else
            {
                requestNamespace = namespaceResult.Value;
            }

            return requestNamespace;
        }
    }
}