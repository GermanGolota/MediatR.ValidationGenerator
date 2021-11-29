using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MediatR.ValidationGenerator.Extensions
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
