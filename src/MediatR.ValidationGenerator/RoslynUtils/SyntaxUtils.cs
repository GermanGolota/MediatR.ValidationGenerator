using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace MediatR.ValidationGenerator.RoslynUtils
{
    internal static class SyntaxUtils
    {
        public static bool InheritsFrom(TypeDeclarationSyntax declaration, string className)
        {
            var types = declaration?.BaseList?.Types;
            bool result;
            if (types.HasValue)
            {
                result = types.Value.Any(x =>
                {
                    string baseClassName = x.ToString();
                    return IsTheSameClassNameOrGeneric(className, baseClassName);
                });
            }
            else
            {
                result = false;
            }
            return result;
        }

        //TODO: Fix this returning wrong result when having a generic and non generic types
        public static bool IsTheSameClassNameOrGeneric(string className, string baseClassName)
        {
            return baseClassName.Equals(className) || baseClassName.StartsWith($"{className}<");
        }
    }
}
