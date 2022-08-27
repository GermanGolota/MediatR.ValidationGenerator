using MediatR.ValidationGenerator.Builders;
using MediatR.ValidationGenerator.Extensions;
using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ValidationGenerator.Rules;

public class CustomValidatorRule : AttributeRule
{
    public override string AttributeName => GlobalNames.CustomValidatorAttribute;

    public override SuccessOrFailure AppendFor(
        IPropertySymbol prop, AttributeData attribute,
        MethodBodyBuilder body, ServicesContainer services)
    {
        var (type, method) = GetArgs(attribute);
        SuccessOrFailure result;
        if (type is not null && method is not null)
        {
            string parName = RequestValidatorCreator.VALIDATOR_PARAMETER_NAME;
            string propName = prop.Name;
            string fullProp = $"{parName}.{propName}";
            string serviceName = services.GetServiceNameFor(type);
            var methods = type.GetAllMethods();
            var methodSymbol = methods
                .Where(x => x.Name == method)
                .FirstOrDefault();

            if (methodSymbol != default)
            {
                var returnType = methodSymbol.ReturnType;
                var isTask = returnType.MetadataName.Equals("Task`1");
                if (returnType is INamedTypeSymbol taskSymbol && taskSymbol.IsGenericType)
                {
                    var taskGenericType = taskSymbol.TypeArguments.First();
                    if (taskGenericType.SpecialType == SpecialType.System_Boolean)
                    {
                        body.AppendNotEnding($"if((await {serviceName}.{method}({fullProp})) == false)");
                        body.AppendError($"nameof({fullProp})", "\"Custom validation failed\"", true);
                        result = true;
                    }
                    else
                    {
                        result = SuccessOrFailure.CreateFailure("Validator method should return Task<bool>");
                    }
                }
                else
                {
                    if (returnType.SpecialType == SpecialType.System_Boolean)
                    {
                        body.AppendNotEnding($"if({serviceName}.{method}({fullProp}) == false)");
                        body.AppendError($"nameof({fullProp})", "\"Custom validation failed\"", true);
                        result = true;
                    }
                    else
                    {
                        result = SuccessOrFailure.CreateFailure("Specified validation method does not return bool or Task<bool>");
                    }
                }
            }
            else
            {
                result = SuccessOrFailure.CreateFailure("Cannot find method with specified name");
            }
        }
        else
        {
            result = SuccessOrFailure.CreateFailure("Type or method name was not provided for Custom Validator Attribute");
        }
        return result;
    }

    public override IEnumerable<ITypeSymbol> GetRequiredServices(AttributeData attribute)
    {
        var type = GetArgs(attribute).type;
        IEnumerable<ITypeSymbol> result;
        if (type is null)
        {
            result = Enumerable.Empty<ITypeSymbol>();
        }
        else
        {
            result = new[] { type };
        }
        return result;
    }

    private (ITypeSymbol? type, string? method) GetArgs(AttributeData attribute)
    {
        var args = attribute.ConstructorArguments;

        var typeVal = args[0].Value;
        var methodName = args[1].Value;
        (ITypeSymbol? type, string? method) result;
        if (typeVal is ITypeSymbol && methodName is string)
        {
            result = (typeVal as ITypeSymbol, methodName as string);
        }
        else
        {
            result = (null, null);
        }
        return result;
    }
}
