﻿using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ValidationGenerator
{
    public static class RequestValidatorCreator
    {
        public static readonly string VALIDATE_METHOD_NAME = "Validate";
        public static readonly string VALIDATOR_PARAMETER_NAME = "value";
        public static readonly string VALIDATOR_ERRORS_LIST_NAME = "failures";
        public static readonly string VALIDATOR_VALIDITY_NAME = "isValid";

        public static (string sourceCode, List<string> failures) CreateValidatorFor(RequestValidationModel model)
        {
            string requestClassName = model.RequestClass.MetadataName;
            string requestNamespace = model.RequestClass.ContainingNamespace.ToDisplayString();
            string requestGlobalName = requestClassName.GetFromGlobal(requestNamespace);

            List<string> failures = new List<string>();
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

                                        body.AppendNotEnding($"#region {prop.Name}Validation");
                                        List<SuccessOrFailure> results = AttributeService.AppendRulesForAttribute(body, prop, attributes);
                                        var msgs = results.Where(x => x.IsFailure).Select(x => x.FailureMessage!);
                                        failures.AddRange(msgs);
                                        body.AppendNotEnding($"#endregion");
                                    }

                                    body.AppendLine($"return new {GlobalNames.ValidationResult}({VALIDATOR_VALIDITY_NAME}, {VALIDATOR_ERRORS_LIST_NAME})");

                                    return body;
                                });
                     });

            string src = classBuilder.Build();

            return (src, failures);
        }
    }
}