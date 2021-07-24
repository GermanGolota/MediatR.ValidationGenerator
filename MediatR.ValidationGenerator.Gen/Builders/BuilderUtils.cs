﻿using MediatR.ValidationGenerator.Gen.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.Builders
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
                if (parameter.DefaultValue.IsNotEmpty())
                {
                    parameterStr += $" = {parameter.DefaultValue}";
                }
                parameterStrs.Add(parameterStr);
            }
            return String.Join(", ", parameterStrs);
        }
    }
}