using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Extensions;
using System.Collections.Immutable;

namespace MediatR.ValidationGenerator;

public static class RegistrationsCreator
{
    public static (string src, string fileName) CreateFor(ImmutableArray<RequestValidationModel> validators)
    {
        string src = ClassBuilder.Create()
            .WithClassName(GlobalNames.DIRegistrationsLocal)
            .WithNamespace(GlobalNames.InternalNamespace)
            .AsPartial()
            .WithAccessModifier(AccessModifier.Internal)
            .WithConstructor(ctor =>
            {
                return ctor
                    .WithBody(body =>
                    {
                        foreach (var validator in validators)
                        {
                            string interfaceType = $"typeof({GlobalNames.Validator}<{validator.RequestClass.GetGlobalName()}>)";
                            var implementationType = $"typeof({validator.ValidatorName.GetFromGlobal(GlobalNames.ValidatorsNamespace)})";
                            body.AppendLine($"this.{GlobalNames.DIRegistrationsDict}.Add({interfaceType}, {implementationType})");
                        }

                        return body;
                    });
            })
            .Build();

        return (src, $"{GlobalNames.DIRegistrationsLocal}.g.cs");
    }
}
