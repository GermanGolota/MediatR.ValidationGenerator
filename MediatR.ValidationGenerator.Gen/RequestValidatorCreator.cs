using FluentValidation;
using FluentValidation.Results;
using MediatR.ValidationGenerator.Gen.Builders;
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
                             .WithParameter("ValidationContext<string>", "context")
                             .WithParameter("CancellationToken", "cancellation", "default")
                             .WithBody((initialMarginBody) =>
                             {
                                 var methodBuilder = new MethodBodyBuilder(initialMarginBody);

                                 foreach (var entry in model.PropertyToSupportedAttributes)
                                 {
                                     var prop = entry.Key;
                                     var attributes = entry.Value;
                                     methodBuilder.AppendLine($"RuleFor(x => x.{prop.Identifier})", endLine: false);

                                     foreach (var attribute in attributes)
                                     {
                                         var attributeRule = AttributeService.CreateRuleForAttribute(attribute);
                                         if (attributeRule.HasValue)
                                         {
                                             methodBuilder.AppendLine(attributeRule.Value, 1);
                                         }
                                     }
                                 }

                                 methodBuilder.AppendLine("return base.ValidateAsync(context, cancellation)");

                                 return methodBuilder;
                             });
                     });

            return classBuilder.Build();
        }
    }


    public class Validator : AbstractValidator<string>
    {
        public override Task<ValidationResult> ValidateAsync(
            ValidationContext<string> context,
            CancellationToken cancellation = default)
        {
            RuleFor(x => x.Length)
                .NotEmpty();
            return base.ValidateAsync(context, cancellation);
        }
    }
}
