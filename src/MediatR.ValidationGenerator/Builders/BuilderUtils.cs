using MediatR.ValidationGenerator.Extensions;
using System;
using System.Collections.Generic;

namespace MediatR.ValidationGenerator.Builders
{
    public static class BuilderUtils
    {
        public const string TAB = "    ";

        public static string BuildParameterList(List<MethodParameter> parameters)
        {
            List<string> parameterStrs = new List<string>();
            foreach (var parameter in parameters)
            {
                string parameterStr = $"{parameter.Type} {parameter.Name}";
                if (parameter.DefaultValue is not null)
                {
                    parameterStr += $" = {parameter.DefaultValue}";
                }
                parameterStrs.Add(parameterStr);
            }
            return string.Join(", ", parameterStrs);
        }
    }
}
