using MediatR.ValidationGenerator.Gen.Builders;
using MediatR.ValidationGenerator.Gen.Extensions;
using MediatR.ValidationGenerator.Gen.Models;
using MediatR.ValidationGenerator.Gen.RoslynUtils;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ValidationGenerator.Gen
{
    public static class RequestValidatorCreator
    {
        public const string VALIDATORS_NAMESPACE = "Validators.Generated";
        public static ValueOrNull<string> CreateValidatorFor(RequestValidationModel model)
        {
            string requestClassName = model.RequestClass.Identifier.ToString();
            string requestNamespace = GetRequestNamespace(model);

            var classBuilder = new ClassBuilder()
                     .WithClassName(model.ValidatorName)
                     .WithNamespace(VALIDATORS_NAMESPACE)
                     .UsingNamespace("FluentValidation")
                     .UsingNamespace("System")
                     .UsingNamespace(requestNamespace)
                     .Implementing($"AbstractValidator<{requestClassName}>")
                     .WithConstructor(ctor =>
                     {
                         return ctor.WithBody((body) =>
                         {
                             foreach (var entry in model.PropertyToSupportedAttributes)
                             {
                                 var prop = entry.Key;
                                 var attributes = entry.Value;
                                 body.AppendLine($"RuleFor(x => x.{prop.Identifier})", endLine: false);

                                 List<string> rules = attributes
                                                .Select(attribute => AttributeService.CreateRulesForAttribute(attribute))
                                                .Flatten()
                                                .ToList();

                                 for (int i = 0; i < rules.Count; i++)
                                 {
                                     bool isLastRule = i == rules.Count - 1;
                                     var rule = rules[i];
                                     body.AppendLine(rule, 1, isLastRule);
                                 }
                             }

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