using MediatR.ValidationGenerator.Builders;

namespace MediatR.ValidationGenerator.Extensions;

public static class MethodBodyBuilderExtensions
{
    public static void AppendNotEnding(this MethodBodyBuilder builder, string line, int margin = 0)
    {
        builder.AppendLine(line, margin, false);
    }

    public static void AppendError(this MethodBodyBuilder body, string propName, string message, bool includeBraces, int initialMargin = 0)
    {
        if (includeBraces)
        {
            body.AppendNotEnding("{", initialMargin);
        }
        body.AppendLine(
            $"{RequestValidatorCreator.VALIDATOR_ERRORS_LIST_NAME}.Add(new {GlobalNames.ValidationFailure}({propName}, {message}))",
            initialMargin + 1);
        body.AppendLine($"{RequestValidatorCreator.VALIDATOR_VALIDITY_NAME} = false", initialMargin + 1);
        if (includeBraces)
        {
            body.AppendNotEnding("}", initialMargin);
        }
    }
}
