using MediatR.ValidationGenerator.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator
{
    public static class GlobalNames
    {
        public static readonly string InternalNamespace = "MediatR.ValidationGenerator.Internal";

        public static readonly string ValidationFailure = "ValidationFailure"
            .GetFromGlobal(InternalNamespace);

        public static readonly string String = "String"
           .GetFromGlobal("System");
    }
}
