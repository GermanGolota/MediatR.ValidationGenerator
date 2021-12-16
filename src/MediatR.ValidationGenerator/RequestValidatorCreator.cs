using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Models;
using MediatR.ValidationGenerator.Extensions;

namespace MediatR.ValidationGenerator
{
    public static class RequestValidatorCreator
    {
        public static readonly string VALIDATE_METHOD_NAME = "Validate";
        public static readonly string VALIDATOR_PARAMETER_NAME = "value";
        public static readonly string VALIDATOR_ERRORS_LIST_NAME = "failures";
        public static readonly string VALIDATOR_VALIDITY_NAME = "isValid";

        public static ValueOrNull<string> CreateValidatorFor(RequestValidationModel model)
        {
            string requestClassName = model.RequestClass.MetadataName;
            string requestNamespace = model.RequestClass.ContainingNamespace.ToDisplayString();
            string requestGlobalName = requestClassName.GetFromGlobal(requestNamespace);

            var classBuilder = ClassBuilder.Create()
                     .WithClassName(model.ValidatorName)
                     .WithNamespace(GlobalNames.ValidatorsNamespace)
                     .Implementing($"{GlobalNames.Validator}<{requestGlobalName}>")
                     .WithMethod(method =>
                     {
                         return method.WithName(VALIDATE_METHOD_NAME)
                                .WithReturnType(GlobalNames.ValidationResult)
                                .WithParameter(requestGlobalName, VALIDATOR_PARAMETER_NAME)
                                .WithBody(body =>
                                {
                                    body
                                       .AppendLine($"bool {VALIDATOR_VALIDITY_NAME} = true")
                                       .AppendLine($"{GlobalNames.List}<{GlobalNames.ValidationFailure}> {VALIDATOR_ERRORS_LIST_NAME} = new {GlobalNames.List}<{GlobalNames.ValidationFailure}>()");

                                    foreach (var entry in model.PropertyToSupportedAttributes)
                                    {
                                        var prop = entry.Key;
                                        var attributes = entry.Value;

                                        body.AppendLine($"#region {prop.Name}Validation");
                                        //TODO: diagnostics
                                        var results = AttributeService.AppendRulesForAttribute(body, prop, attributes);
                                        body.AppendLine($"#endregion");
                                    }

                                    body.AppendLine($"return new {GlobalNames.ValidationResult}({VALIDATOR_VALIDITY_NAME}, {VALIDATOR_ERRORS_LIST_NAME})");

                                    return body;
                                });
                     });

            return classBuilder.Build();
        }
    }
}