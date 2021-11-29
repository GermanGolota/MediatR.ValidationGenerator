﻿using MediatR.ValidationGenerator.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace MediatR.ValidationGenerator.RoslynUtils
{
    internal static class SyntaxUtils
    {
        public static bool InheritsFrom(TypeDeclarationSyntax declaration, string className)
        {
            return declaration.BaseList.Types.Any(x =>
            {
                string baseClassName = x.ToString();
                return IsTheSameClassNameOrGeneric(className, baseClassName);
            });
        }

        //TODO: Fix this returning wrong result when having a generic and non generic types
        public static bool IsTheSameClassNameOrGeneric(string className, string baseClassName)
        {
            return baseClassName.Equals(className) || baseClassName.StartsWith($"{className}<");
        }

        public static ValueOrNull<string> GetNamespace(ClassDeclarationSyntax classSyntax)
        {
            ValueOrNull<string> result;
            if (classSyntax.Parent is NamespaceDeclarationSyntax nameSpace)
            {
                result = nameSpace.Name.ToString();
            }
            else
            {
                result = ValueOrNull<string>.CreateNull("Not in a namespace");
            }

            return result;
        }
    }
}
