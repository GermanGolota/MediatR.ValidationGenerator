using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.Extensions
{
    internal static class RoslynExtensions
    {
        public static bool IsNotAbstract(this TypeDeclarationSyntax syntax)
        {
            var members = syntax.Modifiers;
            return members.None(x => x.ToString() == "abstract");
        }

        public static bool IsConcreate(this TypeDeclarationSyntax syntax)
        {
            return syntax.IsNotAbstract() && !(syntax is InterfaceDeclarationSyntax);
        }
    }
}
