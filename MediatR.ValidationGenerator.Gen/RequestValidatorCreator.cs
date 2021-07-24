using FluentValidation;
using FluentValidation.Results;
using MediatR.ValidationGenerator.Gen.Builders;
using MediatR.ValidationGenerator.Gen.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ValidationGenerator.Gen
{
    public static class RequestValidatorCreator
    {
        public const string VALIDATORS_NAMESPACE = "Validators.Generated";
        public static ValueOrNull<string> CreateValidatorFor(RequestValidationModel model)
        {
            string requestClassName = model.RequestClass.Identifier.ToString();
            var classBuilder = new ClassBuilder()
                     .WithClassName(model.ValidatorName)
                     .WithNamespace(VALIDATORS_NAMESPACE)
                     .UsingNamespace("FluentValidation")
                     .Implementing($"AbstractValidator<{requestClassName}>")
                     .WithMethod((initialMargin) =>
                     {
                         return new MethodBuilder(initialMargin)
                             .AsOverride()
                             //.AsAsync()
                             .WithReturnType("Task<ValidationResult>")
                             .WithName("ValidateAsync")
                             .WithParameter($"ValidationContext<{requestClassName}>", "context")
                             .WithParameter("CancellationToken", "cancellation", "default")
                             .WithBody((initialMarginBody) =>
                             {
                                 var methodBuilder = new MethodBodyBuilder(initialMarginBody);

                                 foreach (var entry in model.PropertyToSupportedAttributes)
                                 {
                                     var prop = entry.Key;
                                     var attributes = entry.Value;
                                     methodBuilder.AppendLine($"RuleFor(x => x.{prop.Identifier})", endLine: false);

                                     List<string> rules = attributes
                                                    .Select(attribute => AttributeService.CreateRulesForAttribute(attribute))
                                                    .Flatten()
                                                    .ToList();

                                     for (int i = 0; i < rules.Count; i++)
                                     {
                                         bool isLastRule = i == rules.Count - 1;
                                         var rule = rules[i];
                                         methodBuilder.AppendLine(rule, 1, isLastRule);
                                     }
                                 }

                                 methodBuilder.AppendLine("return base.ValidateAsync(context, cancellation)");

                                 return methodBuilder;
                             });
                     });

            return classBuilder.Build();
        }
    }
}
